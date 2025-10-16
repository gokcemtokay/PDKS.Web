using PDKS.Business.DTOs;
using PDKS.Data.Entities;
using PDKS.Data.Repositories;

namespace PDKS.Business.Services
{
    public class ParametreService : IParametreService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ParametreService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ParametreListDTO>> GetAllAsync()
        {
            var parametreler = await _unitOfWork.Parametreler.GetAllAsync();
            return parametreler.Select(p => new ParametreListDTO
            {
                Id = p.Id,
                Ad = p.Ad,
                Deger = p.Deger,
                Birim = p.Birim,
                Aciklama = p.Aciklama,
                Kategori = p.Kategori
            }).OrderBy(p => p.Kategori).ThenBy(p => p.Ad);
        }

        public async Task<ParametreListDTO> GetByIdAsync(int id)
        {
            var parametre = await _unitOfWork.Parametreler.GetByIdAsync(id);
            if (parametre == null)
                throw new Exception("Parametre bulunamadı");

            return new ParametreListDTO
            {
                Id = parametre.Id,
                Ad = parametre.Ad,
                Deger = parametre.Deger,
                Birim = parametre.Birim,
                Aciklama = parametre.Aciklama,
                Kategori = parametre.Kategori
            };
        }

        public async Task<string> GetDegerAsync(string ad)
        {
            var parametre = (await _unitOfWork.Parametreler.FindAsync(p => p.Ad == ad)).FirstOrDefault();
            return parametre?.Deger ?? string.Empty;
        }

        public async Task<int> CreateAsync(ParametreCreateDTO dto)
        {
            // Aynı isimde parametre var mı kontrol et
            var mevcutParametre = await _unitOfWork.Parametreler.FindAsync(p => p.Ad == dto.Ad);
            if (mevcutParametre.Any())
                throw new Exception("Bu isimde bir parametre zaten bulunmaktadır");

            var parametre = new Parametre
            {
                Ad = dto.Ad,
                Deger = dto.Deger,
                Birim = dto.Birim,
                Aciklama = dto.Aciklama,
                Kategori = dto.Kategori
            };

            await _unitOfWork.Parametreler.AddAsync(parametre);
            await _unitOfWork.SaveChangesAsync();

            return parametre.Id;
        }

        public async Task UpdateAsync(ParametreUpdateDTO dto)
        {
            var parametre = await _unitOfWork.Parametreler.GetByIdAsync(dto.Id);
            if (parametre == null)
                throw new Exception("Parametre bulunamadı");

            // Aynı isimde başka parametre var mı kontrol et (kendisi hariç)
            var mevcutParametre = await _unitOfWork.Parametreler.FindAsync(p =>
                p.Ad == dto.Ad && p.Id != dto.Id);
            if (mevcutParametre.Any())
                throw new Exception("Bu isimde bir parametre zaten bulunmaktadır");

            parametre.Ad = dto.Ad;
            parametre.Deger = dto.Deger;
            parametre.Birim = dto.Birim;
            parametre.Aciklama = dto.Aciklama;
            parametre.Kategori = dto.Kategori;

            _unitOfWork.Parametreler.Update(parametre);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var parametre = await _unitOfWork.Parametreler.GetByIdAsync(id);
            if (parametre == null)
                throw new Exception("Parametre bulunamadı");

            _unitOfWork.Parametreler.Remove(parametre);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}