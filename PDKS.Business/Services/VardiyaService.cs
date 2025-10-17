using PDKS.Business.DTOs;
using PDKS.Data.Entities;
using PDKS.Data.Repositories;

namespace PDKS.Business.Services
{
    public class VardiyaService : IVardiyaService
    {
        private readonly IUnitOfWork _unitOfWork;

        public VardiyaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<VardiyaListDTO>> GetAllAsync()
        {
            var vardiyalar = await _unitOfWork.Vardiyalar.GetAllAsync();
            return vardiyalar.Select(v => new VardiyaListDTO
            {
                Id = v.Id,
                Ad = v.Ad,
                BaslangicSaati = v.BaslangicSaati.ToString(@"hh\:mm"),
                BitisSaati = v.BitisSaati.ToString(@"hh\:mm"),
                GeceVardiyasiMi = v.GeceVardiyasiMi,
                EsnekVardiyaMi = v.EsnekVardiyaMi,
                ToleransSuresiDakika = v.ToleransSuresiDakika,
                Aciklama = v.Aciklama,
                Durum = v.Durum
            }).OrderBy(v => v.Ad);
        }

        public async Task<IEnumerable<VardiyaListDTO>> GetAktifVardiyalarAsync()
        {
            var vardiyalar = await _unitOfWork.Vardiyalar.FindAsync(v => v.Durum);
            return vardiyalar.Select(v => new VardiyaListDTO
            {
                Id = v.Id,
                Ad = v.Ad,
                BaslangicSaati = v.BaslangicSaati.ToString(@"hh\:mm"),
                BitisSaati = v.BitisSaati.ToString(@"hh\:mm"),
                GeceVardiyasiMi = v.GeceVardiyasiMi,
                EsnekVardiyaMi = v.EsnekVardiyaMi,
                ToleransSuresiDakika = v.ToleransSuresiDakika,
                Aciklama = v.Aciklama,
                Durum = v.Durum
            }).OrderBy(v => v.Ad);
        }

        public async Task<VardiyaDetailDTO> GetByIdAsync(int id)
        {
            var vardiya = await _unitOfWork.Vardiyalar.GetByIdAsync(id);
            if (vardiya == null)
                throw new Exception("Vardiya bulunamadı");

            return new VardiyaDetailDTO
            {
                Id = vardiya.Id,
                Ad = vardiya.Ad,
                BaslangicSaati = vardiya.BaslangicSaati.ToString(@"hh\:mm"),
                BitisSaati = vardiya.BitisSaati.ToString(@"hh\:mm"),
                GeceVardiyasiMi = vardiya.GeceVardiyasiMi,
                EsnekVardiyaMi = vardiya.EsnekVardiyaMi,
                ToleransSuresiDakika = vardiya.ToleransSuresiDakika,
                Aciklama = vardiya.Aciklama,
                Durum = vardiya.Durum
            };
        }

        public async Task<int> CreateAsync(VardiyaCreateDTO dto)
        {
            // Parse time strings
            if (!TimeSpan.TryParse(dto.BaslangicSaati, out var baslangic))
                throw new Exception("Geçersiz başlangıç saati formatı");

            if (!TimeSpan.TryParse(dto.BitisSaati, out var bitis))
                throw new Exception("Geçersiz bitiş saati formatı");

            var vardiya = new Vardiya
            {
                Ad = dto.Ad,
                BaslangicSaati = baslangic,
                BitisSaati = bitis,
                GeceVardiyasiMi = dto.GeceVardiyasiMi,
                EsnekVardiyaMi = dto.EsnekVardiyaMi,
                ToleransSuresiDakika = dto.ToleransSuresiDakika,
                Aciklama = dto.Aciklama,
                Durum = dto.Durum
            };

            await _unitOfWork.Vardiyalar.AddAsync(vardiya);
            await _unitOfWork.SaveChangesAsync();

            return vardiya.Id;
        }

        public async Task UpdateAsync(VardiyaUpdateDTO dto)
        {
            var vardiya = await _unitOfWork.Vardiyalar.GetByIdAsync(dto.Id);
            if (vardiya == null)
                throw new Exception("Vardiya bulunamadı");

            // Parse time strings
            if (!TimeSpan.TryParse(dto.BaslangicSaati, out var baslangic))
                throw new Exception("Geçersiz başlangıç saati formatı");

            if (!TimeSpan.TryParse(dto.BitisSaati, out var bitis))
                throw new Exception("Geçersiz bitiş saati formatı");

            vardiya.Ad = dto.Ad;
            vardiya.BaslangicSaati = baslangic;
            vardiya.BitisSaati = bitis;
            vardiya.GeceVardiyasiMi = dto.GeceVardiyasiMi;
            vardiya.EsnekVardiyaMi = dto.EsnekVardiyaMi;
            vardiya.ToleransSuresiDakika = dto.ToleransSuresiDakika;
            vardiya.Aciklama = dto.Aciklama;
            vardiya.Durum = dto.Durum;

            _unitOfWork.Vardiyalar.Update(vardiya);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var vardiya = await _unitOfWork.Vardiyalar.GetByIdAsync(id);
            if (vardiya == null)
                throw new Exception("Vardiya bulunamadı");

            // Check if any personnel is using this shift
            var personellerWithVardiya = await _unitOfWork.Personeller.FindAsync(p => p.VardiyaId == id);
            if (personellerWithVardiya.Any())
                throw new Exception("Bu vardiyada personel bulunduğu için silinemez");

            _unitOfWork.Vardiyalar.Remove(vardiya);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}