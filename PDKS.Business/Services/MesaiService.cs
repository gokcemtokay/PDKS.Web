using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Office2010.PowerPoint;
using PDKS.Business.DTOs;
using PDKS.Data.Entities;
using PDKS.Data.Repositories;

namespace PDKS.Business.Services
{
    public class MesaiService : IMesaiService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MesaiService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<MesaiListDTO>> GetAllAsync()
        {
            var mesailer = await _unitOfWork.Mesailer.GetAllAsync();
            var personeller = await _unitOfWork.Personeller.GetAllAsync();
            var kullanicilar = await _unitOfWork.Kullanicilar.GetAllAsync();

            return mesailer.Select(m => new MesaiListDTO
            {
                Id = m.Id,
                PersonelId = m.PersonelId,
                PersonelAdi = personeller.FirstOrDefault(p => p.Id == m.PersonelId)?.AdSoyad ?? "",
                PersonelSicilNo = personeller.FirstOrDefault(p => p.Id == m.PersonelId)?.SicilNo ?? "",
                Tarih = m.Tarih,
                BaslangicSaati = m.BaslangicSaati,
                BitisSaati = m.BitisSaati,
                ToplamSaat = m.ToplamSaat,
                FazlaMesaiSaati = m.FazlaMesaiSaati,
                MesaiTipi = m.MesaiTipi,
                OnayDurumu = m.OnayDurumu,
                OnaylayanKullaniciAdi = m.OnaylayanKullaniciId.HasValue
                    ? kullanicilar.FirstOrDefault(k => k.Id == m.OnaylayanKullaniciId)?.KullaniciAdi
                    : null,
                OnayTarihi = m.OnayTarihi,
                Aciklama = m.Aciklama
            }).OrderByDescending(m => m.Tarih);
        }

        public async Task<MesaiListDTO> GetByIdAsync(int id)
        {
            var mesai = await _unitOfWork.Mesailer.GetByIdAsync(id);
            if (mesai == null)
                throw new Exception("Mesai kaydı bulunamadı");

            var personel = await _unitOfWork.Personeller.GetByIdAsync(mesai.PersonelId);
            Kullanici? onaylayan = null;
            if (mesai.OnaylayanKullaniciId.HasValue)
            {
                onaylayan = await _unitOfWork.Kullanicilar.GetByIdAsync(mesai.OnaylayanKullaniciId.Value);
            }

            return new MesaiListDTO
            {
                Id = mesai.Id,
                PersonelId = mesai.PersonelId,
                PersonelAdi = personel?.AdSoyad ?? "",
                PersonelSicilNo = personel?.SicilNo ?? "",
                Tarih = mesai.Tarih,
                BaslangicSaati = mesai.BaslangicSaati,
                BitisSaati = mesai.BitisSaati,
                ToplamSaat = mesai.ToplamSaat,
                FazlaMesaiSaati = mesai.FazlaMesaiSaati,
                MesaiTipi = mesai.MesaiTipi,
                OnayDurumu = mesai.OnayDurumu,
                OnaylayanKullaniciAdi = onaylayan?.KullaniciAdi,
                OnayTarihi = mesai.OnayTarihi,
                Aciklama = mesai.Aciklama
            };
        }

        public async Task<int> CreateAsync(MesaiCreateDTO dto)
        {
            // Mesai süresi hesaplama
            var toplamSaat = CalculateTotalHours(dto.BaslangicSaati, dto.BitisSaati);

            // Mesai tipini belirle
            var mesaiTipi = await DetermineMesaiTipi(dto.Tarih);

            var mesai = new Mesai
            {
                PersonelId = dto.PersonelId,
                Tarih = dto.Tarih.Date,
                BaslangicSaati = dto.BaslangicSaati,
                BitisSaati = dto.BitisSaati,
                ToplamSaat = toplamSaat,
                FazlaMesaiSaati = await CalculateFazlaMesai(dto.PersonelId, dto.Tarih, toplamSaat),
                MesaiTipi = mesaiTipi,
                OnayDurumu = "Beklemede",
                Aciklama = dto.Aciklama
            };

            await _unitOfWork.Mesailer.AddAsync(mesai);
            await _unitOfWork.SaveChangesAsync();

            return mesai.Id;
        }

        public async Task UpdateAsync(MesaiUpdateDTO dto)
        {
            var mesai = await _unitOfWork.Mesailer.GetByIdAsync(dto.Id);
            if (mesai == null)
                throw new Exception("Mesai kaydı bulunamadı");

            if (mesai.OnayDurumu == "Onaylandı")
                throw new Exception("Onaylanmış mesai kaydı düzenlenemez");

            var toplamSaat = CalculateTotalHours(dto.BaslangicSaati, dto.BitisSaati);
            var mesaiTipi = await DetermineMesaiTipi(dto.Tarih);

            mesai.PersonelId = dto.PersonelId;
            mesai.Tarih = dto.Tarih.Date;
            mesai.BaslangicSaati = dto.BaslangicSaati;
            mesai.BitisSaati = dto.BitisSaati;
            mesai.ToplamSaat = toplamSaat;
            mesai.FazlaMesaiSaati = await CalculateFazlaMesai(dto.PersonelId, dto.Tarih, toplamSaat);
            mesai.MesaiTipi = mesaiTipi;
            mesai.Aciklama = dto.Aciklama;

            _unitOfWork.Mesailer.Update(mesai);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var mesai = await _unitOfWork.Mesailer.GetByIdAsync(id);
            if (mesai == null)
                throw new Exception("Mesai kaydı bulunamadı");

            if (mesai.OnayDurumu == "Onaylandı")
                throw new Exception("Onaylanmış mesai kaydı silinemez");

            _unitOfWork.Mesailer.Remove(mesai);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task OnaylaAsync(int id, int onaylayanKullaniciId)
        {
            var mesai = await _unitOfWork.Mesailer.GetByIdAsync(id);
            if (mesai == null)
                throw new Exception("Mesai kaydı bulunamadı");

            if (mesai.OnayDurumu != "Beklemede")
                throw new Exception("Sadece bekleyen mesai kayıtları onaylanabilir");

            mesai.OnayDurumu = "Onaylandı";
            mesai.OnaylayanKullaniciId = onaylayanKullaniciId;
            mesai.OnayTarihi = DateTime.UtcNow;
            mesai.RedNedeni = null;

            _unitOfWork.Mesailer.Update(mesai);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task ReddetAsync(int id, int onaylayanKullaniciId, string redNedeni)
        {
            var mesai = await _unitOfWork.Mesailer.GetByIdAsync(id);
            if (mesai == null)
                throw new Exception("Mesai kaydı bulunamadı");

            if (mesai.OnayDurumu != "Beklemede")
                throw new Exception("Sadece bekleyen mesai kayıtları reddedilebilir");

            mesai.OnayDurumu = "Reddedildi";
            mesai.OnaylayanKullaniciId = onaylayanKullaniciId;
            mesai.OnayTarihi = DateTime.UtcNow;
            mesai.RedNedeni = redNedeni;

            _unitOfWork.Mesailer.Update(mesai);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<MesaiListDTO>> GetBekleyenMesailerAsync()
        {
            var mesailer = await _unitOfWork.Mesailer.FindAsync(m => m.OnayDurumu == "Beklemede");
            var personeller = await _unitOfWork.Personeller.GetAllAsync();

            return mesailer.Select(m => new MesaiListDTO
            {
                Id = m.Id,
                PersonelId = m.PersonelId,
                PersonelAdi = personeller.FirstOrDefault(p => p.Id == m.PersonelId)?.AdSoyad ?? "",
                PersonelSicilNo = personeller.FirstOrDefault(p => p.Id == m.PersonelId)?.SicilNo ?? "",
                Tarih = m.Tarih,
                BaslangicSaati = m.BaslangicSaati,
                BitisSaati = m.BitisSaati,
                ToplamSaat = m.ToplamSaat,
                FazlaMesaiSaati = m.FazlaMesaiSaati,
                MesaiTipi = m.MesaiTipi,
                OnayDurumu = m.OnayDurumu,
                Aciklama = m.Aciklama
            }).OrderBy(m => m.Tarih);
        }

        // Helper methods
        private decimal CalculateTotalHours(TimeSpan baslangic, TimeSpan bitis)
        {
            var fark = bitis - baslangic;
            if (fark < TimeSpan.Zero)
                fark = fark.Add(TimeSpan.FromDays(1));

            return (decimal)fark.TotalHours;
        }

        private async Task<string> DetermineMesaiTipi(DateTime tarih)
        {
            // Hafta sonu kontrolü
            if (tarih.DayOfWeek == DayOfWeek.Saturday || tarih.DayOfWeek == DayOfWeek.Sunday)
                return "HaftaSonu";

            // Resmi tatil kontrolü
            var tatiller = await _unitOfWork.Tatiller.FindAsync(t => t.Tarih.Date == tarih.Date);
            if (tatiller.Any())
                return "ResmiTatil";

            return "Normal";
        }

        private async Task<decimal> CalculateFazlaMesai(int personelId, DateTime tarih, decimal toplamSaat)
        {
            var gunlukCalismaSaati = 8m; // Bu parametreden alınabilir

            if (toplamSaat > gunlukCalismaSaati)
                return toplamSaat - gunlukCalismaSaati;

            return 0;
        }

        public async Task<IEnumerable<MesaiListDTO>> GetBySirketAsync(int sirketId)
        {
            var mesailer = await _unitOfWork.Mesailer.FindAsync(x => x.SirketId == sirketId);
            var personeller = await _unitOfWork.Personeller.GetAllAsync();
            var kullanicilar = await _unitOfWork.Kullanicilar.GetAllAsync();

            return mesailer.Select(m => new MesaiListDTO
            {
                Id = m.Id,
                PersonelId = m.PersonelId,
                PersonelAdi = personeller.FirstOrDefault(p => p.Id == m.PersonelId)?.AdSoyad ?? "",
                PersonelSicilNo = personeller.FirstOrDefault(p => p.Id == m.PersonelId)?.SicilNo ?? "",
                Tarih = m.Tarih,
                BaslangicSaati = m.BaslangicSaati,
                BitisSaati = m.BitisSaati,
                ToplamSaat = m.ToplamSaat,
                FazlaMesaiSaati = m.FazlaMesaiSaati,
                MesaiTipi = m.MesaiTipi,
                OnayDurumu = m.OnayDurumu,
                OnaylayanKullaniciAdi = m.OnaylayanKullaniciId.HasValue
                    ? kullanicilar.FirstOrDefault(k => k.Id == m.OnaylayanKullaniciId)?.KullaniciAdi
                    : null,
                OnayTarihi = m.OnayTarihi,
                Aciklama = m.Aciklama
            }).OrderByDescending(m => m.Tarih);
        }


    }
}
