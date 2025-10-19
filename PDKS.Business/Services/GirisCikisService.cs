using PDKS.Business.DTOs;
using PDKS.Data.Entities;
using PDKS.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDKS.Business.Services
{
    public class GirisCikisService : IGirisCikisService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GirisCikisService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> CreateAsync(GirisCikisCreateDTO dto)
        {
            var girisCikis = new GirisCikis
            {
                PersonelId = dto.PersonelId,
                GirisZamani = dto.GirisZamani,
                CikisZamani = dto.CikisZamani,
                CihazId = dto.CihazId,
                ElleGiris = dto.ElleGiris,
                Not = dto.Not,
                Durum = "Manuel Giriş", // Veya duruma göre bir mantık kurulabilir
                OlusturmaTarihi = DateTime.UtcNow
            };

            await _unitOfWork.GirisCikislar.AddAsync(girisCikis);
            await _unitOfWork.SaveChangesAsync();
            return girisCikis.Id;
        }

        public async Task DeleteAsync(int id)
        {
            var girisCikis = await _unitOfWork.GirisCikislar.GetByIdAsync(id);
            if (girisCikis == null)
                throw new Exception("Kayıt bulunamadı");

            _unitOfWork.GirisCikislar.Remove(girisCikis);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<GirisCikisListDTO>> GetAllAsync()
        {
            var girisCikislar = await _unitOfWork.GirisCikislar.GetAllAsync();
            return girisCikislar.Select(g => new GirisCikisListDTO
            {
                Id = g.Id,
                PersonelId = g.PersonelId,
                PersonelAdi = g.Personel?.AdSoyad,
                SicilNo = g.Personel?.SicilNo,
                GirisZamani = g.GirisZamani,
                CikisZamani = g.CikisZamani,
                Durum = g.Durum
            }).OrderByDescending(g => g.GirisZamani);
        }

        public async Task<GirisCikisListDTO> GetByIdAsync(int id)
        {
            var g = await _unitOfWork.GirisCikislar.GetByIdAsync(id);
            if (g == null)
                return null;

            return new GirisCikisListDTO
            {
                Id = g.Id,
                PersonelId = g.PersonelId,
                PersonelAdi = g.Personel?.AdSoyad,
                SicilNo = g.Personel?.SicilNo,
                GirisZamani = g.GirisZamani,
                CikisZamani = g.CikisZamani,
                Durum = g.Durum,
                Not = g.Not
            };
        }

        // YENİ EKLENEN METOT
        public async Task<IEnumerable<GirisCikisListDTO>> GetByDateAsync(DateTime date)
        {
            var girisCikislar = await _unitOfWork.GirisCikislar.FindAsync(g => g.GirisZamani.HasValue && g.GirisZamani.Value.Date == date.Date);
            return girisCikislar.Select(g => new GirisCikisListDTO
            {
                Id = g.Id,
                PersonelId = g.PersonelId,
                PersonelAdi = g.Personel?.AdSoyad,
                SicilNo = g.Personel?.SicilNo,
                GirisZamani = g.GirisZamani,
                CikisZamani = g.CikisZamani,
                Durum = g.Durum,
                Not = g.Not
            }).OrderByDescending(g => g.GirisZamani);
        }


        public async Task UpdateAsync(GirisCikisUpdateDTO dto)
        {
            var girisCikis = await _unitOfWork.GirisCikislar.GetByIdAsync(dto.Id);
            if (girisCikis == null)
                throw new Exception("Kayıt bulunamadı");

            girisCikis.GirisZamani = dto.GirisZamani;
            girisCikis.CikisZamani = dto.CikisZamani;
            girisCikis.Not = dto.Not;
            // ElleGiris gibi alanlar genellikle değiştirilmez ama ihtiyaca göre eklenebilir.
            girisCikis.GuncellemeTarihi = DateTime.UtcNow;

            _unitOfWork.GirisCikislar.Update(girisCikis);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}