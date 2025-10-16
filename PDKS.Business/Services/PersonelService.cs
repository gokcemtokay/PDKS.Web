using PDKS.Business.DTOs;
using PDKS.Data.Entities;
using PDKS.Data.Repositories;

namespace PDKS.Business.Services
{
    // Personel Service Implementation
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
            return personeller.Select(p => new PersonelListDTO
            {
                Id = p.Id,
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
            return personeller.Select(p => new PersonelListDTO
            {
                Id = p.Id,
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

        public async Task<PersonelDetailDTO> GetByIdAsync(int id)
        {
            var personel = await _unitOfWork.Personeller.GetByIdAsync(id);
            if (personel == null)
                throw new Exception("Personel bulunamadı");

            var departman = personel.DepartmanId.HasValue
                ? await _unitOfWork.Departmanlar.GetByIdAsync(personel.DepartmanId.Value)
                : null;

            var vardiya = personel.VardiyaId.HasValue
                ? await _unitOfWork.Vardiyalar.GetByIdAsync(personel.VardiyaId.Value)
                : null;

            var avanslar = await _unitOfWork.Avanslar.FindAsync(a => a.PersonelId == id);
            var primler = await _unitOfWork.Primler.FindAsync(p => p.PersonelId == id);

            return new PersonelDetailDTO
            {
                Id = personel.Id,
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
                Maas = personel.Maas ?? 0m,
                Unvan = personel.Unvan,
                Gorev = personel.Gorev,
                AvansLimiti = personel.AvansLimiti ?? 0m,
                DepartmanId = personel.DepartmanId,
                DepartmanAdi = departman?.Ad,
                Departman = departman?.Ad ?? "Belirtilmemiş",  // Geriye dönük uyumluluk
                VardiyaId = personel.VardiyaId,
                VardiyaAdi = vardiya?.Ad,
                Durum = personel.Durum,
                Notlar = personel.Notlar,
                KayitTarihi = personel.KayitTarihi,
                AktifAvansSayisi = avanslar.Count(a => a.Durum == "Aktif"),
                ToplamAvans = avanslar.Sum(a => (decimal?)a.Tutar) ?? 0m,
                ToplamPrim = primler.Sum(p => (decimal?)p.Tutar) ?? 0m
            };
        }

        public async Task<int> CreateAsync(PersonelCreateDTO dto)
        {
            // Check if email or sicil no exists
            var emailExists = await _unitOfWork.Personeller.AnyAsync(p => p.Email == dto.Email);
            if (emailExists)
                throw new Exception("Bu email adresi zaten kullanılıyor");

            var sicilExists = await _unitOfWork.Personeller.AnyAsync(p => p.SicilNo == dto.SicilNo);
            if (sicilExists)
                throw new Exception("Bu sicil numarası zaten kullanılıyor");

            var personel = new Personel
            {
                AdSoyad = dto.AdSoyad,
                SicilNo = dto.SicilNo,
                TcKimlikNo = dto.TcKimlikNo,
                DepartmanId = dto.DepartmanId,  // ✅ DÜZELTILDI
                Gorev = dto.Gorev,
                Email = dto.Email,
                Telefon = dto.Telefon,
                Adres = dto.Adres,
                DogumTarihi = dto.DogumTarihi,
                Cinsiyet = dto.Cinsiyet,
                KanGrubu = dto.KanGrubu,
                Durum = dto.Durum,
                GirisTarihi = dto.GirisTarihi,
                CikisTarihi = dto.CikisTarihi,
                VardiyaId = dto.VardiyaId,
                Maas = dto.Maas,
                Unvan = dto.Unvan,
                AvansLimiti = dto.AvansLimiti,
                Notlar = dto.Notlar,
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

            // Check email uniqueness (excluding current record)
            var emailExists = await _unitOfWork.Personeller.AnyAsync(p => p.Email == dto.Email && p.Id != dto.Id);
            if (emailExists)
                throw new Exception("Bu email adresi zaten kullanılıyor");

            // Check sicil no uniqueness (excluding current record)
            var sicilExists = await _unitOfWork.Personeller.AnyAsync(p => p.SicilNo == dto.SicilNo && p.Id != dto.Id);
            if (sicilExists)
                throw new Exception("Bu sicil numarası zaten kullanılıyor");

            personel.AdSoyad = dto.AdSoyad;
            personel.SicilNo = dto.SicilNo;
            personel.TcKimlikNo = dto.TcKimlikNo;
            personel.DepartmanId = dto.DepartmanId;  // ✅ DÜZELTILDI
            personel.Gorev = dto.Gorev;
            personel.Email = dto.Email;
            personel.Telefon = dto.Telefon;
            personel.Adres = dto.Adres;
            personel.DogumTarihi = dto.DogumTarihi;
            personel.Cinsiyet = dto.Cinsiyet;
            personel.KanGrubu = dto.KanGrubu;
            personel.Durum = dto.Durum;
            personel.GirisTarihi = dto.GirisTarihi;
            personel.CikisTarihi = dto.CikisTarihi;
            personel.VardiyaId = dto.VardiyaId;
            personel.Maas = dto.Maas;
            personel.Unvan = dto.Unvan;
            personel.AvansLimiti = dto.AvansLimiti;
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
                (p.Departman != null && p.Departman.Ad.Contains(searchTerm)) ||  // ✅ DÜZELTILDI
                p.Email.Contains(searchTerm));

            return personeller.Select(p => new PersonelListDTO
            {
                Id = p.Id,
                AdSoyad = p.AdSoyad,
                SicilNo = p.SicilNo,
                Departman = p.Departman?.Ad ?? string.Empty,  // ✅ DÜZELTILDI
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

            return personeller.Select(p => new PersonelListDTO
            {
                Id = p.Id,
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
    }
}