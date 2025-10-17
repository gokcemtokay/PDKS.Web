using PDKS.Business.DTOs;
using PDKS.Data.Entities;
using PDKS.Data.Repositories;


namespace PDKS.Business.Services
{
    public class KullaniciService : IKullaniciService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;

        public KullaniciService(IUnitOfWork unitOfWork, IAuthService authService)
        {
            _unitOfWork = unitOfWork;
            _authService = authService;
        }

        public async Task<IEnumerable<KullaniciListDTO>> GetAllAsync()
        {
            var kullanicilar = await _unitOfWork.Kullanicilar.GetAllAsync();
            var personeller = await _unitOfWork.Personeller.GetAllAsync();
            var roller = await _unitOfWork.Roller.GetAllAsync();

            return kullanicilar.Select(k => new KullaniciListDTO
            {
                Id = k.Id,
                KullaniciAdi = k.KullaniciAdi,
                PersonelId = k.PersonelId,
                PersonelAdi = k.PersonelId.HasValue ? personeller.FirstOrDefault(p => p.Id == k.PersonelId)?.AdSoyad : null,
                PersonelSicilNo = k.PersonelId.HasValue ? personeller.FirstOrDefault(p => p.Id == k.PersonelId)?.SicilNo : null,
                RolId = k.RolId,
                RolAdi = roller.FirstOrDefault(r => r.Id == k.RolId)?.RolAdi ?? "Bilinmiyor",
                Aktif = k.Aktif,
                SonGirisTarihi = k.SonGirisTarihi
            }).OrderBy(k => k.KullaniciAdi);
        }

        public async Task<KullaniciDetailDTO> GetByIdAsync(int id)
        {
            var kullanici = await _unitOfWork.Kullanicilar.GetByIdAsync(id);
            if (kullanici == null)
                throw new Exception("Kullanıcı bulunamadı");

            var rol = await _unitOfWork.Roller.GetByIdAsync(kullanici.RolId);
            Personel? personel = null;
            if (kullanici.PersonelId.HasValue)
            {
                personel = await _unitOfWork.Personeller.GetByIdAsync(kullanici.PersonelId.Value);
            }

            return new KullaniciDetailDTO
            {
                Id = kullanici.Id,
                KullaniciAdi = kullanici.KullaniciAdi,
                PersonelId = kullanici.PersonelId,
                PersonelAdi = personel?.AdSoyad,
                PersonelSicilNo = personel?.SicilNo,
                PersonelEmail = personel?.Email,
                RolId = kullanici.RolId,
                RolAdi = rol?.RolAdi ?? "Bilinmiyor",
                Aktif = kullanici.Aktif,
                SonGirisTarihi = kullanici.SonGirisTarihi,
                KayitTarihi = kullanici.KayitTarihi
            };
        }

        public async Task<int> CreateAsync(KullaniciCreateDTO dto)
        {
            // Kullanıcı adı kontrolü
            if (await KullaniciAdiVarMiAsync(dto.KullaniciAdi))
                throw new Exception("Bu kullanıcı adı zaten kullanılıyor");

            // Personel zaten bir kullanıcıya bağlı mı kontrolü
            if (dto.PersonelId.HasValue)
            {
                var mevcutKullanici = await _unitOfWork.Kullanicilar.FindAsync(k => k.PersonelId == dto.PersonelId);
                if (mevcutKullanici.Any())
                    throw new Exception("Bu personel zaten bir kullanıcıya bağlı");
            }

            var kullanici = new Kullanici
            {
                KullaniciAdi = dto.KullaniciAdi,
                Sifre = _authService.HashPassword(dto.Sifre),
                PersonelId = dto.PersonelId,
                RolId = dto.RolId,
                Email = dto.KullaniciAdi,
                Aktif = dto.Aktif,
                KayitTarihi = DateTime.UtcNow
            };

            await _unitOfWork.Kullanicilar.AddAsync(kullanici);
            await _unitOfWork.SaveChangesAsync();

            return kullanici.Id;
        }

        public async Task UpdateAsync(KullaniciUpdateDTO dto)
        {
            var kullanici = await _unitOfWork.Kullanicilar.GetByIdAsync(dto.Id);
            if (kullanici == null)
                throw new Exception("Kullanıcı bulunamadı");

            // Kullanıcı adı kontrolü (kendisi hariç)
            if (await KullaniciAdiVarMiAsync(dto.KullaniciAdi, dto.Id))
                throw new Exception("Bu kullanıcı adı zaten kullanılıyor");

            // Personel kontrolü (kendisi hariç)
            if (dto.PersonelId.HasValue)
            {
                var mevcutKullanici = await _unitOfWork.Kullanicilar.FindAsync(k =>
                    k.PersonelId == dto.PersonelId && k.Id != dto.Id);
                if (mevcutKullanici.Any())
                    throw new Exception("Bu personel zaten başka bir kullanıcıya bağlı");
            }

            kullanici.KullaniciAdi = dto.KullaniciAdi;
            kullanici.PersonelId = dto.PersonelId;
            kullanici.RolId = dto.RolId;
            kullanici.Aktif = dto.Aktif;

            _unitOfWork.Kullanicilar.Update(kullanici);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var kullanici = await _unitOfWork.Kullanicilar.GetByIdAsync(id);
            if (kullanici == null)
                throw new Exception("Kullanıcı bulunamadı");

            // Son admin kontrolü
            var roller = await _unitOfWork.Roller.GetAllAsync();
            var adminRolId = roller.FirstOrDefault(r => r.RolAdi == "Admin")?.Id;

            if (adminRolId.HasValue && kullanici.RolId == adminRolId.Value)
            {
                var adminSayisi = (await _unitOfWork.Kullanicilar.FindAsync(k => k.RolId == adminRolId.Value)).Count();
                if (adminSayisi <= 1)
                    throw new Exception("Sistemde en az bir admin kullanıcı olmalıdır");
            }

            _unitOfWork.Kullanicilar.Remove(kullanici);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task SifreDegistirAsync(int id, string yeniSifre)
        {
            if (string.IsNullOrWhiteSpace(yeniSifre) || yeniSifre.Length < 6)
                throw new Exception("Şifre en az 6 karakter olmalıdır");

            var kullanici = await _unitOfWork.Kullanicilar.GetByIdAsync(id);
            if (kullanici == null)
                throw new Exception("Kullanıcı bulunamadı");

            kullanici.Sifre = _authService.HashPassword(yeniSifre);
            _unitOfWork.Kullanicilar.Update(kullanici);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> KullaniciAdiVarMiAsync(string kullaniciAdi, int? excludeId = null)
        {
            var kullanicilar = await _unitOfWork.Kullanicilar.FindAsync(k => k.KullaniciAdi == kullaniciAdi);

            if (excludeId.HasValue)
                kullanicilar = kullanicilar.Where(k => k.Id != excludeId.Value);

            return kullanicilar.Any();
        }
    }
}