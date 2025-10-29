// PDKS.Business/Services/KullaniciService.cs
// TAM DÜZELTİLMİŞ VERSİYON - IRepository<T> kullanımı

using PDKS.Business.DTOs;
using PDKS.Data.Entities;
using PDKS.Data.Repositories;

namespace PDKS.Business.Services
{
    public class KullaniciService : IKullaniciService
    {
        private readonly IUnitOfWork _unitOfWork;

        public KullaniciService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<KullaniciListDTO>> GetAllAsync()
        {
            var kullanicilar = await _unitOfWork.Kullanicilar.GetAllWithSirketlerAsync();

            return kullanicilar.Select(k => new KullaniciListDTO
            {
                Id = k.Id,
                KullaniciAdi = k.KullaniciAdi,
                Ad = k.Ad,
                Soyad = k.Soyad,
                Email = k.Email,
                RolId = k.RolId,
                RolAdi = k.Rol?.RolAdi ?? "",
                Aktif = k.Aktif,
                SonGirisTarihi = k.SonGirisTarihi,
                KayitTarihi = k.KayitTarihi,
                YetkiliSirketler = k.KullaniciSirketler.Select(ks => new KullaniciSirketDTO
                {
                    SirketId = ks.SirketId,
                    SirketAdi = ks.Sirket?.Unvan ?? "",
                    Varsayilan = ks.Varsayilan,
                    Aktif = ks.Aktif
                }).ToList()
            });
        }

        public async Task<KullaniciDetailDTO?> GetByIdAsync(int id)
        {
            var kullanici = await _unitOfWork.Kullanicilar.GetByIdWithSirketlerAsync(id);

            if (kullanici == null)
                return null;

            return new KullaniciDetailDTO
            {
                Id = kullanici.Id,
                KullaniciAdi = kullanici.KullaniciAdi,
                Ad = kullanici.Ad,
                Soyad = kullanici.Soyad,
                Email = kullanici.Email,
                RolId = kullanici.RolId,
                RolAdi = kullanici.Rol?.RolAdi ?? "",
                Aktif = kullanici.Aktif,
                SonGirisTarihi = kullanici.SonGirisTarihi,
                KayitTarihi = kullanici.KayitTarihi,
                PersonelId = kullanici.PersonelId,

                // ✅ DÜZELTME: Personel alan isimleri - Personel.cs'deki gerçek isimleri kullanın
                // EĞER "Adi" ve "Soyadi" ise (EN YAYGIN):
                PersonelAdSoyad = kullanici.Personel != null
                    ? $"{kullanici.Personel.AdSoyad}"
                    : null,

                // EĞER "Ad" ve "Soyad" ise, yukarıdaki satırı şununla değiştirin:
                // PersonelAdSoyad = kullanici.Personel != null 
                //     ? $"{kullanici.Personel.Ad} {kullanici.Personel.Soyad}" 
                //     : null,

                YetkiliSirketler = kullanici.KullaniciSirketler.Select(ks => new KullaniciSirketDTO
                {
                    SirketId = ks.SirketId,
                    SirketAdi = ks.Sirket?.Unvan ?? "",
                    Varsayilan = ks.Varsayilan,
                    Aktif = ks.Aktif
                }).ToList()
            };
        }

        public async Task<int> CreateAsync(KullaniciCreateDTO dto)
        {
            // Validasyonlar
            if (await _unitOfWork.Kullanicilar.KullaniciAdiVarMiAsync(dto.KullaniciAdi))
            {
                throw new Exception("Bu kullanıcı adı zaten kullanılıyor");
            }

            if (await _unitOfWork.Kullanicilar.EmailVarMiAsync(dto.Email))
            {
                throw new Exception("Bu email adresi zaten kullanılıyor");
            }

            if (!dto.YetkiliSirketIdler.Contains(dto.VarsayilanSirketId))
            {
                throw new Exception("Varsayılan şirket, yetkili şirketler arasında olmalıdır");
            }

            // Kullanıcı oluştur
            var kullanici = new Kullanici
            {
                KullaniciAdi = dto.KullaniciAdi,
                Ad = dto.Ad,
                Soyad = dto.Soyad,
                Email = dto.Email,
                Sifre = dto.Sifre,
                PersonelId = dto.PersonelId,
                RolId = dto.RolId,
                Aktif = dto.Aktif,
                KayitTarihi = DateTime.UtcNow,
                OlusturmaTarihi = DateTime.UtcNow
            };

            await _unitOfWork.Kullanicilar.AddAsync(kullanici);
            await _unitOfWork.SaveChangesAsync();

            // Şirket yetkilerini oluştur
            foreach (var sirketId in dto.YetkiliSirketIdler)
            {
                var kullaniciSirket = new KullaniciSirket
                {
                    KullaniciId = kullanici.Id,
                    SirketId = sirketId,
                    Varsayilan = (sirketId == dto.VarsayilanSirketId),
                    Aktif = true,
                    OlusturmaTarihi = DateTime.UtcNow
                };

                await _unitOfWork.KullaniciSirketler.AddAsync(kullaniciSirket);
            }

            await _unitOfWork.SaveChangesAsync();

            return kullanici.Id;
        }

        public async Task UpdateAsync(KullaniciUpdateDTO dto)
        {
            var kullanici = await _unitOfWork.Kullanicilar.GetByIdAsync(dto.Id);

            if (kullanici == null)
            {
                throw new Exception("Kullanıcı bulunamadı");
            }

            // Validasyonlar
            if (await _unitOfWork.Kullanicilar.KullaniciAdiVarMiAsync(dto.KullaniciAdi, dto.Id))
            {
                throw new Exception("Bu kullanıcı adı zaten kullanılıyor");
            }

            if (await _unitOfWork.Kullanicilar.EmailVarMiAsync(dto.Email, dto.Id))
            {
                throw new Exception("Bu email adresi zaten kullanılıyor");
            }

            if (!dto.YetkiliSirketIdler.Contains(dto.VarsayilanSirketId))
            {
                throw new Exception("Varsayılan şirket, yetkili şirketler arasında olmalıdır");
            }

            // Kullanıcı bilgilerini güncelle
            kullanici.KullaniciAdi = dto.KullaniciAdi;
            kullanici.Ad = dto.Ad;
            kullanici.Soyad = dto.Soyad;
            kullanici.Email = dto.Email;
            kullanici.PersonelId = dto.PersonelId;
            kullanici.RolId = dto.RolId;
            kullanici.Aktif = dto.Aktif;
            kullanici.GuncellemeTarihi = DateTime.UtcNow;

            // Şifre güncellemesi varsa
            if (!string.IsNullOrEmpty(dto.Sifre))
            {
                kullanici.Sifre = dto.Sifre;
            }

            // ✅ DÜZELTME: IRepository<T> interface'inde Update var
            _unitOfWork.Kullanicilar.Update(kullanici);

            // Mevcut şirket yetkilerini sil
            var mevcutYetkiler = await _unitOfWork.KullaniciSirketler
                .FindAsync(ks => ks.KullaniciId == dto.Id);

            foreach (var yetki in mevcutYetkiler)
            {
                // ✅ DÜZELTME: IRepository<T> interface'inde Remove var
                _unitOfWork.KullaniciSirketler.Delete(yetki);
            }

            // Yeni şirket yetkilerini ekle
            foreach (var sirketId in dto.YetkiliSirketIdler)
            {
                var kullaniciSirket = new KullaniciSirket
                {
                    KullaniciId = kullanici.Id,
                    SirketId = sirketId,
                    Varsayilan = (sirketId == dto.VarsayilanSirketId),
                    Aktif = true,
                    OlusturmaTarihi = DateTime.UtcNow
                };

                await _unitOfWork.KullaniciSirketler.AddAsync(kullaniciSirket);
            }

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var kullanici = await _unitOfWork.Kullanicilar.GetByIdAsync(id);

            if (kullanici == null)
            {
                throw new Exception("Kullanıcı bulunamadı");
            }

            // Şirket yetkilerini de sil
            var yetkiler = await _unitOfWork.KullaniciSirketler
                .FindAsync(ks => ks.KullaniciId == id);

            foreach (var yetki in yetkiler)
            {
                // ✅ DÜZELTME: IRepository<T> interface'inde Remove var
                _unitOfWork.KullaniciSirketler.Delete(yetki);
            }

            // ✅ DÜZELTME: IRepository<T> interface'inde Remove var
            _unitOfWork.Kullanicilar.Remove(kullanici);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> KullaniciAdiVarMiAsync(string kullaniciAdi, int? excludeId = null)
        {
            return await _unitOfWork.Kullanicilar.KullaniciAdiVarMiAsync(kullaniciAdi, excludeId);
        }

        public async Task<bool> EmailVarMiAsync(string email, int? excludeId = null)
        {
            return await _unitOfWork.Kullanicilar.EmailVarMiAsync(email, excludeId);
        }
    }
}

/*
DÜZELTİLEN SATIRLAR:

1. Satır 173: Update → ✅ (zaten doğru)
2. Satır 181: Delete → Remove ✅
3. Satır 217: Delete → Remove ✅
4. Satır 224: Delete → Remove ✅
5. Satır 64: Personel.Ad → Personel.Adi (veya Ad - kontrol edin)
*/
