using AutoMapper;
using PDKS.Business.DTOs;
using PDKS.Data.Entities;
using PDKS.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDKS.Business.Services
{
    public class VardiyaService : IVardiyaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public VardiyaService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<VardiyaListDTO>> GetAllAsync()
        {
            var vardiyalar = await _unitOfWork.Vardiyalar.GetAllAsync();
            return _mapper.Map<IEnumerable<VardiyaListDTO>>(vardiyalar);
        }

        public async Task<IEnumerable<VardiyaListDTO>> GetAktifVardiyalarAsync()
        {
            var vardiyalar = await _unitOfWork.Vardiyalar
                .FindAsync(v => v.Durum == true);

            return _mapper.Map<IEnumerable<VardiyaListDTO>>(vardiyalar);
        }

        // ⭐ KRİTİK METOT: Şirket ID'sine göre vardiyaları filtreler
        public async Task<IEnumerable<VardiyaListDTO>> GetBySirketAsync(int sirketId)
        {
            // Vardiya Entity'sindeki SirketId alanına göre filtreleme yapar.
            var vardiyalar = await _unitOfWork.Vardiyalar
                .FindAsync(v => v.SirketId == sirketId); // Vardiya Entity'sinde SirketId olması şarttır.

            return _mapper.Map<IEnumerable<VardiyaListDTO>>(vardiyalar);
        }

        public async Task<VardiyaDetailDTO> GetByIdAsync(int id)
        {
            var vardiya = await _unitOfWork.Vardiyalar.GetByIdAsync(id);

            if (vardiya == null)
            {
                throw new Exception($"ID {id} olan vardiya bulunamadı.");
            }

            return _mapper.Map<VardiyaDetailDTO>(vardiya);
        }

        public async Task<int> CreateAsync(VardiyaCreateDTO dto)
        {
            var vardiya = _mapper.Map<Vardiya>(dto);

            // Varsayılan değerler
            vardiya.Durum = true;
            // DTO'da SirketId olduğu varsayılmıştır.

            await _unitOfWork.Vardiyalar.AddAsync(vardiya);
            await _unitOfWork.SaveChangesAsync();

            return vardiya.Id;
        }

        public async Task UpdateAsync(VardiyaUpdateDTO dto)
        {
            var vardiya = await _unitOfWork.Vardiyalar.GetByIdAsync(dto.Id);

            if (vardiya == null)
            {
                throw new Exception($"ID {dto.Id} olan vardiya bulunamadı.");
            }

            // GÜVENLİK KONTROLÜ
            if (vardiya.SirketId != dto.SirketId)
            {
                throw new UnauthorizedAccessException("Vardiya güncelleme işlemi başarısız: Şirket yetkisi eşleşmiyor.");
            }

            _mapper.Map(dto, vardiya);

            _unitOfWork.Vardiyalar.Update(vardiya);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var vardiya = await _unitOfWork.Vardiyalar.GetByIdAsync(id);

            if (vardiya == null)
            {
                throw new Exception($"ID {id} olan vardiya bulunamadı.");
            }

            // Soft Delete (Durum'u pasif yap)
            vardiya.Durum = false;
            _unitOfWork.Vardiyalar.Update(vardiya);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}