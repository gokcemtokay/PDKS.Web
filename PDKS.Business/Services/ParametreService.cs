using PDKS.Business.DTOs;
using PDKS.Data.Entities;
using PDKS.Data.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDKS.Business.Services
{
    public class ParametreService : IParametreService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ParametreService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<ParametreDTO>> GetAllAsync()
        {
            var parametreler = await _unitOfWork.Parametreler.GetAllAsync();
            return parametreler.Select(p => new ParametreDTO
            {
                Id = p.Id,
                Ad = p.Ad,
                Deger = p.Deger,
                Birim = p.Birim,
                Aciklama = p.Aciklama,
                Kategori = p.Kategori
            }).ToList();
        }

        public async Task<ParametreDTO> GetByIdAsync(int id)
        {
            var parametre = await _unitOfWork.Parametreler.GetByIdAsync(id);
            if (parametre == null)
                return null;

            return new ParametreDTO
            {
                Id = parametre.Id,
                Ad = parametre.Ad,
                Deger = parametre.Deger,
                Birim = parametre.Birim,
                Aciklama = parametre.Aciklama,
                Kategori = parametre.Kategori
            };
        }

        // YENİ METOD - Kategoriye göre getir
        public async Task<List<ParametreDTO>> GetByKategoriAsync(string kategori)
        {
            var parametreler = await _unitOfWork.Parametreler.GetAllAsync();

            return parametreler
                .Where(p => p.Kategori == kategori)
                .Select(p => new ParametreDTO
                {
                    Id = p.Id,
                    Ad = p.Ad,
                    Deger = p.Deger,
                    Birim = p.Birim,
                    Aciklama = p.Aciklama,
                    Kategori = p.Kategori
                })
                .ToList();
        }

        public async Task<int> CreateAsync(ParametreCreateDTO dto)
        {
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

            // Delete yerine Remove kullanın
            _unitOfWork.Parametreler.Remove(parametre);
            await _unitOfWork.SaveChangesAsync();
        }
    
        public async Task<IEnumerable<ParametreListDTO>> GetBySirketAsync(int sirketId)
        {
            var entities = await _unitOfWork.Parametreler.FindAsync(x => x.SirketId == sirketId);
            return entities.Select(p => new ParametreListDTO
            {
                Id = p.Id,
                Ad = p.Ad,
                Deger = p.Deger,
                Birim = p.Birim,
                Aciklama = p.Aciklama,
                Kategori = p.Kategori
            });
        }


    }
}
