using PDKS.Business.DTOs;
using PDKS.Data.Entities;
using PDKS.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDKS.Business.Services
{
    public class CihazService : ICihazService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CihazService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Create ve Update metotları CihazAdi, IPAdres, Lokasyon, Durum alanlarını kullanır ve doğrudur.
        public async Task<int> CreateAsync(CihazCreateDTO dto)
        {
            var cihaz = new Cihaz
            {
                CihazAdi = dto.CihazAdi,
                IPAdres = dto.IPAdres,
                Lokasyon = dto.Lokasyon,
                Durum = dto.Durum,
                OlusturmaTarihi = DateTime.UtcNow
            };
            await _unitOfWork.Cihazlar.AddAsync(cihaz);
            await _unitOfWork.SaveChangesAsync();
            return cihaz.Id;
        }

        public async Task UpdateAsync(CihazUpdateDTO dto)
        {
            var cihaz = await _unitOfWork.Cihazlar.GetByIdAsync(dto.Id);
            if (cihaz == null)
                throw new Exception("Cihaz bulunamadı");

            cihaz.CihazAdi = dto.CihazAdi;
            cihaz.IPAdres = dto.IPAdres;
            cihaz.Lokasyon = dto.Lokasyon;
            cihaz.Durum = dto.Durum;

            _unitOfWork.Cihazlar.Update(cihaz);
            await _unitOfWork.SaveChangesAsync();
        }

        // GetAllAsync, CihazListDTO'daki tüm alanları doldurur.
        public async Task<IEnumerable<CihazListDTO>> GetAllAsync()
        {
            var cihazlar = await _unitOfWork.Cihazlar.GetAllAsync();

            var today = DateTime.Today;
            var bugunkuGirisler = await _unitOfWork.GirisCikislar.FindAsync(g => g.CihazId.HasValue && g.GirisZamani.HasValue && g.GirisZamani.Value.Date == today);
            var okumaSayilari = bugunkuGirisler
                .GroupBy(g => g.CihazId.Value)
                .ToDictionary(grp => grp.Key, grp => grp.Count());

            return cihazlar.Select(c => new CihazListDTO
            {
                Id = c.Id,
                CihazAdi = c.CihazAdi,
                IPAdres = c.IPAdres,
                Lokasyon = c.Lokasyon,
                Durum = c.Durum,
                SonBaglantiZamani = c.SonBaglantiZamani,
                BugunkuOkumaSayisi = okumaSayilari.GetValueOrDefault(c.Id, 0)
            });
        }

        // GetByIdAsync, CihazUpdateDTO'yu doldurur.
        public async Task<CihazUpdateDTO> GetByIdAsync(int id)
        {
            var c = await _unitOfWork.Cihazlar.GetByIdAsync(id);
            if (c == null) return null;

            return new CihazUpdateDTO
            {
                Id = c.Id,
                CihazAdi = c.CihazAdi,
                IPAdres = c.IPAdres,
                Lokasyon = c.Lokasyon,
                Durum = c.Durum
            };
        }

        // GetCihazLoglariAsync, düzeltilmiş CihazLog entity'sini kullanır.
        public async Task<IEnumerable<object>> GetCihazLoglariAsync(int cihazId)
        {
            var loglar = await _unitOfWork.CihazLoglari.FindAsync(l => l.CihazId == cihazId);
            return loglar.Select(l => new
            {
                l.Id,
                LogTarihi = l.Tarih,
                LogMesaji = l.Mesaj,
                LogTipi = l.Tip
            }).OrderByDescending(l => l.LogTarihi);
        }
    }
}