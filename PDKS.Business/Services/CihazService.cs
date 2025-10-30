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

        public async Task<int> CreateAsync(CihazCreateDTO dto)
        {
            var cihaz = new Cihaz
            {
                SirketId = dto.SirketId,
                CihazAdi = dto.CihazAdi,
                CihazTipi = dto.CihazTipi,  // ← YENİ
                IPAdres = dto.IPAdres,
                Port = dto.Port,  // ← YENİ
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
            cihaz.CihazTipi = dto.CihazTipi;  // ← YENİ
            cihaz.IPAdres = dto.IPAdres;
            cihaz.Port = dto.Port;  // ← YENİ
            cihaz.Lokasyon = dto.Lokasyon;
            cihaz.Durum = dto.Durum;

            _unitOfWork.Cihazlar.Update(cihaz);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var cihaz = await _unitOfWork.Cihazlar.GetByIdAsync(id);
            if (cihaz == null)
                throw new Exception("Cihaz bulunamadı");

            _unitOfWork.Cihazlar.Remove(cihaz);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<CihazListDTO>> GetAllAsync()
        {
            var cihazlar = await _unitOfWork.Cihazlar.GetAllAsync();

            var today = DateTime.Today;
            var bugunkuGirisler = await _unitOfWork.GirisCikislar.FindAsync(g =>
                g.CihazId.HasValue &&
                g.GirisZamani.HasValue &&
                g.GirisZamani.Value.Date == today);

            var okumaSayilari = bugunkuGirisler
                .GroupBy(g => g.CihazId.Value)
                .ToDictionary(grp => grp.Key, grp => grp.Count());

            return cihazlar.Select(c => new CihazListDTO
            {
                Id = c.Id,
                SirketId = c.SirketId,
                CihazAdi = c.CihazAdi,
                CihazTipi = c.CihazTipi,  // ← YENİ
                IPAdres = c.IPAdres,
                Port = c.Port,  // ← YENİ
                Lokasyon = c.Lokasyon,
                Durum = c.Durum,
                SonBaglantiZamani = c.SonBaglantiZamani,
                BugunkuOkumaSayisi = okumaSayilari.GetValueOrDefault(c.Id, 0)
            });
        }

        public async Task<CihazUpdateDTO> GetByIdAsync(int id)
        {
            var c = await _unitOfWork.Cihazlar.GetByIdAsync(id);
            if (c == null) return null;

            return new CihazUpdateDTO
            {
                Id = c.Id,
                SirketId = c.SirketId,
                CihazAdi = c.CihazAdi,
                CihazTipi = c.CihazTipi,  // ← YENİ
                IPAdres = c.IPAdres,
                Port = c.Port,  // ← YENİ
                Lokasyon = c.Lokasyon,
                Durum = c.Durum
            };
        }

        public async Task<IEnumerable<object>> GetCihazLoglariAsync(int cihazId)
        {
            var loglar = await _unitOfWork.CihazLoglari.FindAsync(l => l.CihazId == cihazId);
            return loglar.Select(l => new
            {
                l.Id,
                l.CihazId,
                l.Mesaj,
                l.Tarih,
                l.Tip
            }).OrderByDescending(l => l.Tarih);
        }

        public async Task<IEnumerable<CihazListDTO>> GetBySirketAsync(int sirketId)
        {
            var cihazlar = await _unitOfWork.Cihazlar.FindAsync(c => c.SirketId == sirketId);

            var today = DateTime.Today;
            var bugunkuGirisler = await _unitOfWork.GirisCikislar.FindAsync(g =>
                g.CihazId.HasValue &&
                g.GirisZamani.HasValue &&
                g.GirisZamani.Value.Date == today);

            var okumaSayilari = bugunkuGirisler
                .GroupBy(g => g.CihazId.Value)
                .ToDictionary(grp => grp.Key, grp => grp.Count());

            return cihazlar.Select(c => new CihazListDTO
            {
                Id = c.Id,
                SirketId = c.SirketId,
                CihazAdi = c.CihazAdi,
                CihazTipi = c.CihazTipi,  // ← YENİ
                IPAdres = c.IPAdres,
                Port = c.Port,  // ← YENİ
                Lokasyon = c.Lokasyon,
                Durum = c.Durum,
                SonBaglantiZamani = c.SonBaglantiZamani,
                BugunkuOkumaSayisi = okumaSayilari.GetValueOrDefault(c.Id, 0)
            });
        }
    }
}
