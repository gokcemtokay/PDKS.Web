using PDKS.Business.DTOs;
using PDKS.Data.Entities;
using PDKS.Data.Repositories;
using System.Collections.Generic;
using System.Linq; // Select, Count, OrderBy, FirstOrDefault, Any için gerekli
using System.Threading.Tasks; // Tüm metotlar Task döndürdüğü için gerekli
using System; // Exception ve DateTime için gerekli

namespace PDKS.Business.Services
{
    public class PersonelService : IPersonelService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PersonelService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<PersonelListDTO>> GetAllAsync()
        {
            var personeller = await _unitOfWork.Personeller.GetAllAsync();

            // Şirket bilgilerini de dahil et
            var sirketler = await _unitOfWork.Sirketler.GetAllAsync();

            return personeller.Select(p => new PersonelListDTO
            {
                Id = p.Id,
                SirketId = p.SirketId,
                SirketAdi = sirketler.FirstOrDefault(s => s.Id == p.SirketId)?.Unvan ?? "",
                AdSoyad = p.AdSoyad,
                SicilNo = p.SicilNo,
                Departman = p.Departman?.Ad ?? string.Empty,
                Gorev = p.Gorev,
                Email = p.Email,
                Telefon = p.Telefon,
                Durum = p.Durum,
                GirisTarihi = p.GirisTarihi,
                CikisTarihi = p.CikisTarihi,
                VardiyaAdi = p.Vardiya?.Ad
            }).OrderBy(p => p.AdSoyad);
        }

        public async Task<IEnumerable<PersonelListDTO>> GetActivePersonelsAsync()
        {
            var personeller = await _unitOfWork.Personeller.FindAsync(p => p.Durum);
            var sirketler = await _unitOfWork.Sirketler.GetAllAsync();

            return personeller.Select(p => new PersonelListDTO
            {
                Id = p.Id,
                SirketId = p.SirketId,
                SirketAdi = sirketler.FirstOrDefault(s => s.Id == p.SirketId)?.Unvan ?? "",
                AdSoyad = p.AdSoyad,
                SicilNo = p.SicilNo,
                Departman = p.Departman?.Ad ?? string.Empty,
                Gorev = p.Gorev,
                Email = p.Email,
                Telefon = p.Telefon,
                Durum = p.Durum,
                GirisTarihi = p.GirisTarihi,
                VardiyaAdi = p.Vardiya?.Ad
            }).OrderBy(p => p.AdSoyad);
        }

        // ⭐ YENİ METOT İMPLEMENTASYONU: Şirket bazlı filtreleme
        public async Task<IEnumerable<PersonelListDTO>> GetBySirketAsync(int sirketId)
        {
            // Şirket ID'sine göre personelleri filtrele
            var personeller = await _unitOfWork.Personeller.FindAsync(p => p.SirketId == sirketId);

            // Personel listesi için sadece ilgili şirketi bir kez çek
            var sirket = await _unitOfWork.Sirketler.GetByIdAsync(sirketId);

            return personeller.Select(p => new PersonelListDTO
            {
                Id = p.Id,
                SirketId = p.SirketId,
                SirketAdi = sirket?.Unvan ?? "",
                AdSoyad = p.AdSoyad,
                SicilNo = p.SicilNo,
                // Departman ve Vardiya navigasyon property'lerinin Eager Loading ile geldiği varsayılmıştır.
                Departman = p.Departman?.Ad ?? string.Empty,
                Gorev = p.Gorev,
                Email = p.Email,
                Telefon = p.Telefon,
                Durum = p.Durum,
                GirisTarihi = p.GirisTarihi,
                CikisTarihi = p.CikisTarihi,
                VardiyaAdi = p.Vardiya?.Ad
            }).OrderBy(p => p.AdSoyad);
        }

        public async Task<int> GetSirketPersonelSayisiAsync(int sirketId)
        {
            var personeller = await _unitOfWork.Personeller.FindAsync(p => p.SirketId == sirketId && p.Durum);
            return personeller.Count();
        }

        public async Task<PersonelDetailDTO> GetByIdAsync(int id)
        {
            var personel = await _unitOfWork.Personeller.GetByIdAsync(id);
            if (personel == null)
                throw new Exception("Personel bulunamadı");

            // Şirket bilgisini ekle
            var sirket = await _unitOfWork.Sirketler.GetByIdAsync(personel.SirketId);

            var departman = personel.DepartmanId.HasValue
                ? await _unitOfWork.Departmanlar.GetByIdAsync(personel.DepartmanId.Value)
                : null;

            var vardiya = personel.VardiyaId.HasValue
                ? await _unitOfWork.Vardiyalar.GetByIdAsync(personel.VardiyaId.Value)
                : null;

            return new PersonelDetailDTO
            {
                Id = personel.Id,
                SirketId = personel.SirketId,
                SirketAdi = sirket?.Unvan,
                AdSoyad = personel.AdSoyad,
                SicilNo = personel.SicilNo,
                TcKimlikNo = personel.TcKimlikNo,
                Email = personel.Email,
                Telefon = personel.Telefon,
                Adres = personel.Adres,
                DogumTarihi = personel.DogumTarihi,
                Cinsiyet = personel.Cinsiyet,
                KanGrubu = personel.KanGrubu,
                GirisTarihi = personel.GirisTarihi,
                CikisTarihi = personel.CikisTarihi,
                Maas = personel.Maas ?? 0,
                Unvan = personel.Unvan,
                Gorev = personel.Gorev,
                AvansLimiti = personel.AvansLimiti ?? 0,
                Durum = personel.Durum,
                DepartmanId = personel.DepartmanId,
                DepartmanAdi = departman?.Ad,
                Departman = departman?.Ad ?? string.Empty,
                VardiyaId = personel.VardiyaId,
                VardiyaAdi = vardiya?.Ad,
                Notlar = personel.Notlar,
                KayitTarihi = personel.KayitTarihi
            };
        }

        public async Task<int> CreateAsync(PersonelCreateDTO dto)
        {
            // Şirket kontrolü
            var sirket = await _unitOfWork.Sirketler.GetByIdAsync(dto.SirketId);
            if (sirket == null)
                throw new Exception("Şirket bulunamadı");

            if (!sirket.Aktif)
                throw new Exception("Şirket aktif değil");

            // Departman kontrolü - Şirkete ait mi?
            if (dto.DepartmanId.HasValue)
            {
                var departman = await _unitOfWork.Departmanlar.GetByIdAsync(dto.DepartmanId.Value);
                if (departman == null)
                    throw new Exception("Departman bulunamadı");

                if (departman.SirketId != dto.SirketId)
                    throw new Exception("Seçilen departman bu şirkete ait değil");
            }

            // Sicil No kontrolü
            var existingSicilNo = await _unitOfWork.Personeller
                .FirstOrDefaultAsync(p => p.SicilNo == dto.SicilNo);
            if (existingSicilNo != null)
                throw new Exception("Bu sicil numarası zaten kullanılıyor");

            // TC Kimlik No kontrolü
            var existingTc = await _unitOfWork.Personeller
                .FirstOrDefaultAsync(p => p.TcKimlikNo == dto.TcKimlikNo);
            if (existingTc != null)
                throw new Exception("Bu TC Kimlik No zaten kayıtlı");

            // Email kontrolü
            var existingEmail = await _unitOfWork.Personeller
                .FirstOrDefaultAsync(p => p.Email == dto.Email);
            if (existingEmail != null)
                throw new Exception("Bu email adresi zaten kullanılıyor");

            var personel = new Personel
            {
                SirketId = dto.SirketId,
                AdSoyad = dto.AdSoyad,
                SicilNo = dto.SicilNo,
                TcKimlikNo = dto.TcKimlikNo,
                Email = dto.Email,
                Telefon = dto.Telefon,
                Adres = dto.Adres,
                DogumTarihi = dto.DogumTarihi,
                Cinsiyet = dto.Cinsiyet,
                KanGrubu = dto.KanGrubu,
                GirisTarihi = dto.GirisTarihi,
                CikisTarihi = dto.CikisTarihi,
                Maas = dto.Maas,
                Unvan = dto.Unvan,
                Gorev = dto.Gorev,
                DepartmanId = dto.DepartmanId,
                VardiyaId = dto.VardiyaId,
                AvansLimiti = dto.AvansLimiti,
                Durum = true,
                Notlar = dto.Notlar,
                KayitTarihi = DateTime.UtcNow,
                OlusturmaTarihi = DateTime.UtcNow
            };

            await _unitOfWork.Personeller.AddAsync(personel);
            await _unitOfWork.SaveChangesAsync();

            return personel.Id;
        }

        public async Task UpdateAsync(PersonelUpdateDTO dto)
        {
            var personel = await _unitOfWork.Personeller.GetByIdAsync(dto.Id);
            if (personel == null)
                throw new Exception("Personel bulunamadı");

            // Şirket kontrolü
            var sirket = await _unitOfWork.Sirketler.GetByIdAsync(dto.SirketId);
            if (sirket == null)
                throw new Exception("Şirket bulunamadı");

            // Departman kontrolü - Şirkete ait mi?
            if (dto.DepartmanId.HasValue)
            {
                var departman = await _unitOfWork.Departmanlar.GetByIdAsync(dto.DepartmanId.Value);
                if (departman == null)
                    throw new Exception("Departman bulunamadı");

                if (departman.SirketId != dto.SirketId)
                    throw new Exception("Seçilen departman bu şirkete ait değil");
            }

            // Sicil No kontrolü (kendisi hariç)
            var existingSicilNo = await _unitOfWork.Personeller
                .FirstOrDefaultAsync(p => p.SicilNo == dto.SicilNo && p.Id != dto.Id);
            if (existingSicilNo != null)
                throw new Exception("Bu sicil numarası başka bir personel tarafından kullanılıyor");

            // TC Kimlik No kontrolü (kendisi hariç)
            var existingTc = await _unitOfWork.Personeller
                .FirstOrDefaultAsync(p => p.TcKimlikNo == dto.TcKimlikNo && p.Id != dto.Id);
            if (existingTc != null)
                throw new Exception("Bu TC Kimlik No başka bir personel tarafından kullanılıyor");

            // Email kontrolü (kendisi hariç)
            var existingEmail = await _unitOfWork.Personeller
                .FirstOrDefaultAsync(p => p.Email == dto.Email && p.Id != dto.Id);
            if (existingEmail != null)
                throw new Exception("Bu email adresi başka bir personel tarafından kullanılıyor");

            personel.SirketId = dto.SirketId;
            personel.AdSoyad = dto.AdSoyad;
            personel.SicilNo = dto.SicilNo;
            personel.TcKimlikNo = dto.TcKimlikNo;
            personel.Email = dto.Email;
            personel.Telefon = dto.Telefon;
            personel.Adres = dto.Adres;
            personel.DogumTarihi = dto.DogumTarihi;
            personel.Cinsiyet = dto.Cinsiyet;
            personel.KanGrubu = dto.KanGrubu;
            personel.GirisTarihi = dto.GirisTarihi;
            personel.CikisTarihi = dto.CikisTarihi;
            personel.DepartmanId = dto.DepartmanId;
            personel.VardiyaId = dto.VardiyaId;
            personel.Gorev = dto.Gorev;
            personel.Maas = dto.Maas;
            personel.Unvan = dto.Unvan;
            personel.AvansLimiti = dto.AvansLimiti;
            personel.Durum = dto.Durum;
            personel.Notlar = dto.Notlar;
            personel.GuncellemeTarihi = DateTime.UtcNow;

            _unitOfWork.Personeller.Update(personel);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var personel = await _unitOfWork.Personeller.GetByIdAsync(id);
            if (personel == null)
                throw new Exception("Personel bulunamadı");

            // Soft delete - just mark as inactive
            personel.Durum = false;
            personel.CikisTarihi = DateTime.UtcNow;
            personel.GuncellemeTarihi = DateTime.UtcNow;

            _unitOfWork.Personeller.Update(personel);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<PersonelListDTO>> SearchAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllAsync();

            var personeller = await _unitOfWork.Personeller.FindAsync(p =>
                p.AdSoyad.Contains(searchTerm) ||
                p.SicilNo.Contains(searchTerm) ||
                (p.Departman != null && p.Departman.Ad.Contains(searchTerm)) ||
                p.Email.Contains(searchTerm));

            var sirketler = await _unitOfWork.Sirketler.GetAllAsync();

            return personeller.Select(p => new PersonelListDTO
            {
                Id = p.Id,
                SirketId = p.SirketId,
                SirketAdi = sirketler.FirstOrDefault(s => s.Id == p.SirketId)?.Unvan ?? "",
                AdSoyad = p.AdSoyad,
                SicilNo = p.SicilNo,
                Departman = p.Departman?.Ad ?? string.Empty,
                Gorev = p.Gorev,
                Email = p.Email,
                Telefon = p.Telefon,
                Durum = p.Durum,
                GirisTarihi = p.GirisTarihi,
                VardiyaAdi = p.Vardiya?.Ad
            }).OrderBy(p => p.AdSoyad);
        }

        public async Task<IEnumerable<PersonelListDTO>> GetByDepartmentAsync(string departman)
        {
            var personeller = await _unitOfWork.Personeller.FindAsync(p =>
                p.Departman != null &&
                p.Departman.Ad == departman &&
                p.Durum);

            var sirketler = await _unitOfWork.Sirketler.GetAllAsync();

            return personeller.Select(p => new PersonelListDTO
            {
                Id = p.Id,
                SirketId = p.SirketId,
                SirketAdi = sirketler.FirstOrDefault(s => s.Id == p.SirketId)?.Unvan ?? "",
                AdSoyad = p.AdSoyad,
                SicilNo = p.SicilNo,
                Departman = p.Departman?.Ad ?? string.Empty,
                Gorev = p.Gorev,
                Email = p.Email,
                Telefon = p.Telefon,
                Durum = p.Durum,
                GirisTarihi = p.GirisTarihi,
                VardiyaAdi = p.Vardiya?.Ad
            }).OrderBy(p => p.AdSoyad);
        }

        //public async Task<int> GetSirketPersonelSayisiAsync(int sirketId)
        //{
        //    var personeller = await _unitOfWork.Personeller.FindAsync(p => p.SirketId == sirketId && p.Durum);
        //    return personeller.Count();
        //}
    }
}