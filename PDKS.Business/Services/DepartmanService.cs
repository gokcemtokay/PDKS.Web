using PDKS.Business.DTOs;
using PDKS.Data.Entities;
using PDKS.Data.Repositories;

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

            return departmanlar.Select(d => new DepartmanListDTO
            {
                Id = d.Id,
                Ad = d.Ad,
                Kod = d.Kod,
                Aciklama = d.Aciklama,
                UstDepartmanId = d.UstDepartmanId,
                UstDepartmanAdi = d.UstDepartman?.Ad,
                Durum = d.Durum,
                PersonelSayisi = personeller.Count(p => p.DepartmanId == d.Id)
            }).OrderBy(d => d.Ad);
        }

        public async Task<DepartmanListDTO> GetByIdAsync(int id)
        {
            var departman = await _unitOfWork.Departmanlar.GetByIdAsync(id);
            if (departman == null)
                throw new Exception("Departman bulunamadı");

            var personeller = await _unitOfWork.Personeller.FindAsync(p => p.DepartmanId == id);

            return new DepartmanListDTO
            {
                Id = departman.Id,
                Ad = departman.Ad,
                Kod = departman.Kod,
                Aciklama = departman.Aciklama,
                UstDepartmanId = departman.UstDepartmanId,
                UstDepartmanAdi = departman.UstDepartman?.Ad,
                Durum = departman.Durum,
                PersonelSayisi = personeller.Count()
            };
        }

        public async Task<int> CreateAsync(DepartmanCreateDTO dto)
        {
            // Kod kontrolü
            if (!string.IsNullOrEmpty(dto.Kod))
            {
                var mevcutDepartman = await _unitOfWork.Departmanlar.FindAsync(d => d.Kod == dto.Kod);
                if (mevcutDepartman.Any())
                    throw new Exception("Bu departman kodu zaten kullanılıyor");
            }

            var departman = new Departman
            {
                Ad = dto.Ad,
                Kod = dto.Kod,
                Aciklama = dto.Aciklama,
                UstDepartmanId = dto.UstDepartmanId,
                Durum = dto.Durum
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

            // Kendi alt departmanı olarak seçilemez
            if (dto.UstDepartmanId == dto.Id)
                throw new Exception("Bir departman kendi alt departmanı olamaz");

            // Kod kontrolü
            if (!string.IsNullOrEmpty(dto.Kod))
            {
                var mevcutDepartman = await _unitOfWork.Departmanlar.FindAsync(d => d.Kod == dto.Kod && d.Id != dto.Id);
                if (mevcutDepartman.Any())
                    throw new Exception("Bu departman kodu zaten kullanılıyor");
            }

            departman.Ad = dto.Ad;
            departman.Kod = dto.Kod;
            departman.Aciklama = dto.Aciklama;
            departman.UstDepartmanId = dto.UstDepartmanId;
            departman.Durum = dto.Durum;

            _unitOfWork.Departmanlar.Update(departman);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var departman = await _unitOfWork.Departmanlar.GetByIdAsync(id);
            if (departman == null)
                throw new Exception("Departman bulunamadı");

            // Personeli olan departman silinemez
            var personeller = await _unitOfWork.Personeller.FindAsync(p => p.DepartmanId == id);
            if (personeller.Any())
                throw new Exception("Bu departmanda personel bulunduğu için silinemez");

            // Alt departmanı olan departman silinemez
            var altDepartmanlar = await _unitOfWork.Departmanlar.FindAsync(d => d.UstDepartmanId == id);
            if (altDepartmanlar.Any())
                throw new Exception("Bu departmanın alt departmanları bulunduğu için silinemez");

            _unitOfWork.Departmanlar.Remove(departman);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<DepartmanListDTO>> GetAktifDepartmanlarAsync()
        {
            var departmanlar = await _unitOfWork.Departmanlar.FindAsync(d => d.Durum);
            return departmanlar.Select(d => new DepartmanListDTO
            {
                Id = d.Id,
                Ad = d.Ad,
                Kod = d.Kod,
                Aciklama = d.Aciklama,
                UstDepartmanId = d.UstDepartmanId,
                UstDepartmanAdi = d.UstDepartman?.Ad,
                Durum = d.Durum
            }).OrderBy(d => d.Ad);
        }
    }
}