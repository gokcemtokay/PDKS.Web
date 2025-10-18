namespace PDKS.Business.DTOs
{
    // Elle Giriş Rapor DTO
    public class ElleGirisRaporDTO
    {
        public int Id { get; set; }
        public DateTime Tarih { get; set; }
        public int PersonelId { get; set; }
        public string PersonelAdi { get; set; }
        public string SicilNo { get; set; }
        public string Departman { get; set; }
        public DateTime? GirisSaati { get; set; }
        public DateTime? CikisSaati { get; set; }
        public DateTime GirisZamani { get; set; } // ✅ Eklendi
        public DateTime? CikisZamani { get; set; } // ✅ Eklendi
        public int GirisYapanKullaniciId { get; set; }
        public string GirisYapanKullanici { get; set; }
        public DateTime OlusturmaTarihi { get; set; } // ✅ Eklendi
        public string Sebep { get; set; }
        public string OnayDurumu { get; set; } // "Beklemede", "Onaylandı", "Reddedildi"
        public string Not { get; set; }
    }
}
