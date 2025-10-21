using PDKS.Business.DTOs;
using PDKS.Data.Entities;
using PDKS.Data.Repositories;
using System.Collections.Generic;
using System.Linq; // Select, Count, OrderBy, FirstOrDefault, Any için gerekli
using System.Threading.Tasks; // Task için gerekli
using System; // Exception, DateTime, HashSet için gerekli

namespace PDKS.Business.Services
{
    public class DepartmanService : IDepartmanService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepartmanService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<DepartmanListDTO>> GetAllAsync()
        {
            var departmanlar = await _unitOfWork.Departmanlar.GetAllAsync();
            var personeller = await _unitOfWork.Personeller.GetAllAsync();
            var sirketler = await _unitOfWork.Sirketler.GetAllAsync();

            return departmanlar.Select(d => new DepartmanListDTO
            {
                Id = d.Id,
                SirketId = d.SirketId,
                SirketAdi = sirketler.FirstOrDefault(s => s.Id == d.SirketId)?.Unvan ?? "",
                DepartmanAdi = d.Ad,
                Aciklama = d.Aciklama,
                UstDepartmanAdi = d.UstDepartman?.Ad,
                Durum = d.Durum,
                PersonelSayisi = personeller.Count(p => p.DepartmanId == d.Id)
            }).OrderBy(d => d.DepartmanAdi);
        }

        public async Task<DepartmanListDTO> GetByIdAsync(int id)
        {
            // Tekil nesne çekerken ilişki (UstDepartman) dahil edilmelidir.
            var departman = await _unitOfWork.Departmanlar.GetByIdAsync(id);
            if (departman == null)
                throw new Exception("Departman bulunamadı");

            var personeller = await _unitOfWork.Personeller.FindAsync(p => p.DepartmanId == id);
            var sirket = await _unitOfWork.Sirketler.GetByIdAsync(departman.SirketId);

            return new DepartmanListDTO
            {
                Id = departman.Id,
                SirketId = departman.SirketId,
                SirketAdi = sirket?.Unvan ?? "",
                DepartmanAdi = departman.Ad,
                Aciklama = departman.Aciklama,
                UstDepartmanAdi = departman.UstDepartman?.Ad,
                Durum = departman.Durum,
                PersonelSayisi = personeller.Count()
            };
        }

        public async Task<int> CreateAsync(DepartmanCreateDTO dto)
        {
            // Şirket kontrolü
            var sirket = await _unitOfWork.Sirketler.GetByIdAsync(dto.SirketId);
            if (sirket == null)
                throw new Exception("Şirket bulunamadı");

            if (!sirket.Aktif)
                throw new Exception("Şirket aktif değil");

            // Kod kontrolü
            if (!string.IsNullOrEmpty(dto.Kod))
            {
                var mevcutDepartman = await _unitOfWork.Departmanlar
                    .FindAsync(d => d.Kod == dto.Kod && d.SirketId == dto.SirketId);
                if (mevcutDepartman.Any())
                    throw new Exception("Bu departman kodu bu şirkette zaten kullanılıyor");
            }

            // Aynı isimde departman kontrolü (aynı şirkette)
            var existingDepartman = await _unitOfWork.Departmanlar
                .FirstOrDefaultAsync(d => d.Ad == dto.DepartmanAdi && d.SirketId == dto.SirketId);
            if (existingDepartman != null)
                throw new Exception("Bu şirkette aynı isimde bir departman zaten var");

            // Üst departman kontrolü
            if (dto.UstDepartmanId.HasValue)
            {
                var ustDepartman = await _unitOfWork.Departmanlar.GetByIdAsync(dto.UstDepartmanId.Value);
                if (ustDepartman == null)
                    throw new Exception("Üst departman bulunamadı");

                if (ustDepartman.SirketId != dto.SirketId)
                    throw new Exception("Üst departman seçilen şirkete ait değil");
            }

            var departman = new Departman
            {
                SirketId = dto.SirketId,
                Ad = dto.DepartmanAdi,
                Kod = dto.Kod,
                Aciklama = dto.Aciklama,
                UstDepartmanId = dto.UstDepartmanId,
                Durum = dto.Durum,
                KayitTarihi = DateTime.UtcNow,
                OlusturmaTarihi = DateTime.UtcNow
            };

            await _unitOfWork.Departmanlar.AddAsync(departman);
            await _unitOfWork.SaveChangesAsync();

            return departman.Id;
        }

        public async Task UpdateAsync(DepartmanUpdateDTO dto)
        {
            var departman = await _unitOfWork.Departmanlar.GetByIdAsync(dto.Id);
            if (departman == null)
                throw new Exception("Departman bulunamadı");

            // Şirket kontrolü
            var sirket = await _unitOfWork.Sirketler.GetByIdAsync(dto.SirketId);
            if (sirket == null)
                throw new Exception("Şirket bulunamadı");

            // Kendi alt departmanı olarak seçilemez
            if (dto.UstDepartmanId == dto.Id)
                throw new Exception("Bir departman kendi alt departmanı olamaz");

            // Üst departman kontrolü
            if (dto.UstDepartmanId.HasValue)
            {
                var ustDepartman = await _unitOfWork.Departmanlar.GetByIdAsync(dto.UstDepartmanId.Value);
                if (ustDepartman == null)
                    throw new Exception("Üst departman bulunamadı");

                if (ustDepartman.SirketId != dto.SirketId)
                    throw new Exception("Üst departman seçilen şirkete ait değil");

                // Döngüsel referans kontrolü
                if (await IsCyclicReference(dto.Id, dto.UstDepartmanId.Value))
                    throw new Exception("Döngüsel referans oluşturulamaz");
            }

            // Kod kontrolü (kendisi hariç, aynı şirkette)
            if (!string.IsNullOrEmpty(dto.Kod))
            {
                var mevcutDepartman = await _unitOfWork.Departmanlar
                    .FindAsync(d => d.Kod == dto.Kod && d.SirketId == dto.SirketId && d.Id != dto.Id);
                if (mevcutDepartman.Any())
                    throw new Exception("Bu departman kodu bu şirkette zaten kullanılıyor");
            }

            // Aynı isimde departman kontrolü (kendisi hariç, aynı şirkette)
            var existingDepartman = await _unitOfWork.Departmanlar
                .FirstOrDefaultAsync(d => d.Ad == dto.DepartmanAdi && d.SirketId == dto.SirketId && d.Id != dto.Id);
            if (existingDepartman != null)
                throw new Exception("Bu şirkette aynı isimde başka bir departman var");

            departman.SirketId = dto.SirketId;
            departman.Ad = dto.DepartmanAdi;
            departman.Kod = dto.Kod;
            departman.Aciklama = dto.Aciklama;
            departman.UstDepartmanId = dto.UstDepartmanId;
            departman.Durum = dto.Durum;
            departman.GuncellemeTarihi = DateTime.UtcNow;

            _unitOfWork.Departmanlar.Update(departman);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var departman = await _unitOfWork.Departmanlar.GetByIdAsync(id);
            if (departman == null)
                throw new Exception("Departman bulunamadı");

            // Alt departman kontrolü
            var altDepartmanlar = await _unitOfWork.Departmanlar.FindAsync(d => d.UstDepartmanId == id);
            if (altDepartmanlar.Any())
                throw new Exception("Bu departmanın alt departmanları var. Önce alt departmanları silin veya taşıyın");

            // Personel kontrolü
            var personeller = await _unitOfWork.Personeller.FindAsync(p => p.DepartmanId == id);
            if (personeller.Any())
                throw new Exception("Bu departmanda personel var. Önce personelleri başka departmana taşıyın");

            _unitOfWork.Departmanlar.Remove(departman);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<DepartmanListDTO>> GetAktifDepartmanlarAsync()
        {
            var departmanlar = await _unitOfWork.Departmanlar.FindAsync(d => d.Durum == true);
            var personeller = await _unitOfWork.Personeller.GetAllAsync();
            var sirketler = await _unitOfWork.Sirketler.GetAllAsync();

            return departmanlar.Select(d => new DepartmanListDTO
            {
                Id = d.Id,
                SirketId = d.SirketId,
                SirketAdi = sirketler.FirstOrDefault(s => s.Id == d.SirketId)?.Unvan ?? "",
                DepartmanAdi = d.Ad,
                Aciklama = d.Aciklama,
                UstDepartmanAdi = d.UstDepartman?.Ad,
                Durum = d.Durum,
                PersonelSayisi = personeller.Count(p => p.DepartmanId == d.Id)
            }).OrderBy(d => d.DepartmanAdi);
        }

        // ⭐ YENİ METOT İMPLEMENTASYONU: Şirket bazlı filtreleme
        public async Task<IEnumerable<DepartmanListDTO>> GetBySirketAsync(int sirketId)
        {
            // Şirket ID'sine göre departmanları filtrele
            var departmanlar = await _unitOfWork.Departmanlar.FindAsync(d => d.SirketId == sirketId);

            // Personel ve Sirket bilgilerini çek (Optimize edilebilir, ancak mevcut yapıyı koruyor)
            var personeller = await _unitOfWork.Personeller.GetAllAsync();
            var sirket = await _unitOfWork.Sirketler.GetByIdAsync(sirketId);

            return departmanlar.Select(d => new DepartmanListDTO
            {
                Id = d.Id,
                SirketId = d.SirketId,
                SirketAdi = sirket?.Unvan ?? "",
                DepartmanAdi = d.Ad,
                Aciklama = d.Aciklama,
                UstDepartmanAdi = d.UstDepartman?.Ad,
                Durum = d.Durum,
                // Personel sayısını sadece ilgili şirketin departmanları için hesaplamak daha doğru olurdu
                PersonelSayisi = personeller.Count(p => p.DepartmanId == d.Id)
            }).OrderBy(d => d.DepartmanAdi);
        }

        public async Task<int> GetSirketDepartmanSayisiAsync(int sirketId)
        {
            var departmanlar = await _unitOfWork.Departmanlar.FindAsync(d => d.SirketId == sirketId && d.Aktif);
            return departmanlar.Count();
        }

        #region Helper Methods

        private async Task<bool> IsCyclicReference(int departmanId, int ustDepartmanId)
        {
            var visited = new HashSet<int>();
            var currentId = ustDepartmanId;

            while (currentId > 0)
            {
                if (currentId == departmanId)
                    return true;

                if (visited.Contains(currentId))
                    return true;

                visited.Add(currentId);

                var departman = await _unitOfWork.Departmanlar.GetByIdAsync(currentId);
                currentId = departman?.UstDepartmanId ?? 0;
            }

            return false;
        }

        #endregion
    }
}