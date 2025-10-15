using PDKS.Business.DTOs;
using PDKS.Data.Entities;
using PDKS.Data.Repositories;

namespace PDKS.Business.Services
{
    public class GirisCikisService : IGirisCikisService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GirisCikisService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<GirisCikisListDTO>> GetAllAsync()
        {
            var kayitlar = await _unitOfWork.GirisCikislar.GetAllAsync();
            return await MapToListDTO(kayitlar);
        }

        public async Task<IEnumerable<GirisCikisListDTO>> GetByPersonelAsync(int personelId)
        {
            var kayitlar = await _unitOfWork.GirisCikislar.FindAsync(g => g.PersonelId == personelId);
            return await MapToListDTO(kayitlar);
        }

        public async Task<IEnumerable<GirisCikisListDTO>> GetByDateRangeAsync(DateTime baslangic, DateTime bitis)
        {
            var kayitlar = await _unitOfWork.GirisCikislar.FindAsync(g =>
                g.GirisZamani >= baslangic && g.GirisZamani <= bitis);
            return await MapToListDTO(kayitlar);
        }

        public async Task<GirisCikisListDTO> GetByIdAsync(int id)
        {
            var kayit = await _unitOfWork.GirisCikislar.GetByIdAsync(id);
            if (kayit == null) return null;

            return await MapToListDTO(new[] { kayit }).ContinueWith(t => t.Result.FirstOrDefault());
        }

        public async Task<int> CreateAsync(GirisCikisCreateDTO dto)
        {
            var kayit = new GirisCikis
            {
                PersonelId = dto.PersonelId,
                GirisZamani = dto.GirisZamani,
                CikisZamani = dto.CikisZamani,
                Kaynak = dto.Kaynak,
                CihazId = dto.CihazId,
                ElleGiris = dto.ElleGiris,
                Not = dto.Not,
                OlusturmaTarihi = DateTime.UtcNow
            };

            await _unitOfWork.GirisCikislar.AddAsync(kayit);
            await _unitOfWork.SaveChangesAsync();

            // Process attendance calculations
            await ProcessGirisCikisAsync(kayit.Id);

            return kayit.Id;
        }

        public async Task UpdateAsync(GirisCikisUpdateDTO dto)
        {
            var kayit = await _unitOfWork.GirisCikislar.GetByIdAsync(dto.Id);
            if (kayit == null)
                throw new Exception("Kayıt bulunamadı");

            kayit.PersonelId = dto.PersonelId;
            kayit.GirisZamani = dto.GirisZamani;
            kayit.CikisZamani = dto.CikisZamani;
            kayit.Kaynak = dto.Kaynak;
            kayit.CihazId = dto.CihazId;
            kayit.ElleGiris = dto.ElleGiris;
            kayit.Not = dto.Not;

            _unitOfWork.GirisCikislar.Update(kayit);
            await _unitOfWork.SaveChangesAsync();

            // Recalculate attendance
            await ProcessGirisCikisAsync(kayit.Id);
        }

        public async Task DeleteAsync(int id)
        {
            var kayit = await _unitOfWork.GirisCikislar.GetByIdAsync(id);
            if (kayit == null)
                throw new Exception("Kayıt bulunamadı");

            _unitOfWork.GirisCikislar.Remove(kayit);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<int> CalculateCalismaSuresi(DateTime? giris, DateTime? cikis)
        {
            if (!giris.HasValue || !cikis.HasValue)
                return 0;

            var sure = (cikis.Value - giris.Value).TotalMinutes;
            return (int)sure;
        }

        public async Task ProcessGirisCikisAsync(int girisCikisId)
        {
            var kayit = await _unitOfWork.GirisCikislar.GetByIdAsync(girisCikisId);
            if (kayit == null) return;

            var personel = await _unitOfWork.Personeller.GetByIdAsync(kayit.PersonelId);
            if (personel == null || !personel.VardiyaId.HasValue) return;

            var vardiya = await _unitOfWork.Vardiyalar.GetByIdAsync(personel.VardiyaId.Value);
            if (vardiya == null) return;

            // Calculate late arrival
            if (kayit.GirisZamani.HasValue)
            {
                var girisZamani = kayit.GirisZamani.Value.TimeOfDay;
                var beklenenGiris = vardiya.BaslangicSaati;
                var tolerans = TimeSpan.FromMinutes(vardiya.ToleransSuresiDakika);

                if (girisZamani > (beklenenGiris + tolerans))
                {
                    kayit.GecKalmaSuresi = (int)(girisZamani - beklenenGiris).TotalMinutes;
                }
                else
                {
                    kayit.GecKalmaSuresi = 0;
                }
            }

            // Calculate early leave
            if (kayit.CikisZamani.HasValue)
            {
                var cikisZamani = kayit.CikisZamani.Value.TimeOfDay;
                var beklenenCikis = vardiya.BitisSaati;
                var tolerans = TimeSpan.FromMinutes(vardiya.ToleransSuresiDakika);

                if (cikisZamani < (beklenenCikis - tolerans))
                {
                    kayit.ErkenCikisSuresi = (int)(beklenenCikis - cikisZamani).TotalMinutes;
                }
                else
                {
                    kayit.ErkenCikisSuresi = 0;
                }

                // Calculate overtime
                if (cikisZamani > (beklenenCikis + tolerans))
                {
                    kayit.FazlaMesaiSuresi = (int)(cikisZamani - beklenenCikis).TotalMinutes;
                }
                else
                {
                    kayit.FazlaMesaiSuresi = 0;
                }
            }

            // Determine status
            if (kayit.GecKalmaSuresi > 0)
                kayit.Durum = "Geç Kalmış";
            else if (kayit.ErkenCikisSuresi > 0)
                kayit.Durum = "Erken Çıkmış";
            else if (kayit.FazlaMesaiSuresi > 0)
                kayit.Durum = "Fazla Mesai";
            else
                kayit.Durum = "Normal";

            _unitOfWork.GirisCikislar.Update(kayit);
            await _unitOfWork.SaveChangesAsync();
        }

        private async Task<IEnumerable<GirisCikisListDTO>> MapToListDTO(IEnumerable<GirisCikis> kayitlar)
        {
            var result = new List<GirisCikisListDTO>();

            foreach (var kayit in kayitlar)
            {
                var personel = await _unitOfWork.Personeller.GetByIdAsync(kayit.PersonelId);

                var calismaSuresi = "";
                if (kayit.GirisZamani.HasValue && kayit.CikisZamani.HasValue)
                {
                    var sure = await CalculateCalismaSuresi(kayit.GirisZamani, kayit.CikisZamani);
                    var saat = sure / 60;
                    var dakika = sure % 60;
                    calismaSuresi = $"{saat}s {dakika}d";
                }

                result.Add(new GirisCikisListDTO
                {
                    Id = kayit.Id,
                    PersonelId = kayit.PersonelId,
                    PersonelAdi = personel?.AdSoyad,
                    SicilNo = personel?.SicilNo,
                    GirisZamani = kayit.GirisZamani,
                    CikisZamani = kayit.CikisZamani,
                    Kaynak = kayit.Kaynak,
                    FazlaMesaiSuresi = kayit.FazlaMesaiSuresi,
                    GecKalmaSuresi = kayit.GecKalmaSuresi,
                    ErkenCikisSuresi = kayit.ErkenCikisSuresi,
                    Durum = kayit.Durum,
                    ElleGiris = kayit.ElleGiris,
                    Not = kayit.Not,
                    CalismaSuresi = calismaSuresi
                });
            }

            return result.OrderByDescending(r => r.GirisZamani);
        }
    }
}
