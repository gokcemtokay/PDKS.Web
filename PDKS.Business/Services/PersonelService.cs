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
                Departman = p.Departman,
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
                Departman = p.Departman,
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
            if (personel == null) return null;

            var toplamGirisCikis = await _unitOfWork.GirisCikislar.CountAsync(g => g.PersonelId == id);
            var toplamIzin = await _unitOfWork.Izinler.CountAsync(i => i.PersonelId == id);
            var bekleyenIzin = await _unitOfWork.Izinler.CountAsync(i => i.PersonelId == id && i.OnayDurumu == "Beklemede");
            var avanslar = await _unitOfWork.Avanslar.FindAsync(a => a.PersonelId == id && a.Durum == "Ödendi");
            var toplamAvans = avanslar.Sum(a => a.Tutar);

            return new PersonelDetailDTO
            {
                Id = personel.Id,
                AdSoyad = personel.AdSoyad,
                SicilNo = personel.SicilNo,
                Departman = personel.Departman,
                Gorev = personel.Gorev,
                Email = personel.Email,
                Telefon = personel.Telefon,
                Durum = personel.Durum,
                GirisTarihi = personel.GirisTarihi,
                CikisTarihi = personel.CikisTarihi,
                Maas = personel.Maas,
                AvansLimiti = personel.AvansLimiti,
                VardiyaId = personel.VardiyaId,
                VardiyaAdi = personel.Vardiya?.Ad,
                ToplamGirisCikis = toplamGirisCikis,
                ToplamIzin = toplamIzin,
                BekleyenIzin = bekleyenIzin,
                ToplamAvans = toplamAvans
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
                Departman = dto.Departman,
                Gorev = dto.Gorev,
                Email = dto.Email,
                Telefon = dto.Telefon,
                Durum = dto.Durum,
                GirisTarihi = dto.GirisTarihi,
                CikisTarihi = dto.CikisTarihi,
                VardiyaId = dto.VardiyaId,
                Maas = dto.Maas,
                AvansLimiti = dto.AvansLimiti,
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
            personel.Departman = dto.Departman;
            personel.Gorev = dto.Gorev;
            personel.Email = dto.Email;
            personel.Telefon = dto.Telefon;
            personel.Durum = dto.Durum;
            personel.GirisTarihi = dto.GirisTarihi;
            personel.CikisTarihi = dto.CikisTarihi;
            personel.VardiyaId = dto.VardiyaId;
            personel.Maas = dto.Maas;
            personel.AvansLimiti = dto.AvansLimiti;
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
                p.Departman.Contains(searchTerm) ||
                p.Email.Contains(searchTerm));

            return personeller.Select(p => new PersonelListDTO
            {
                Id = p.Id,
                AdSoyad = p.AdSoyad,
                SicilNo = p.SicilNo,
                Departman = p.Departman,
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
            var personeller = await _unitOfWork.Personeller.FindAsync(p => p.Departman == departman && p.Durum);
            return personeller.Select(p => new PersonelListDTO
            {
                Id = p.Id,
                AdSoyad = p.AdSoyad,
                SicilNo = p.SicilNo,
                Departman = p.Departman,
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
