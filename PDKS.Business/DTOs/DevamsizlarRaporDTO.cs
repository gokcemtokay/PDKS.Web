namespace PDKS.Business.DTOs
{
    // Devamsızlar Rapor DTO
    public class DevamsizlarRaporDTO
    {
        public int SirketId { get; set; }
        public DateTime Tarih { get; set; } // ✅ Eklendi
        public int PersonelId { get; set; } // ✅ Eklendi
        public string PersonelAdi { get; set; }
        public string SicilNo { get; set; }
        public string Departman { get; set; }
        public int DevamsizGunSayisi { get; set; }
        public string DevamsizGunler { get; set; }
        public string DevamsizlikSebep { get; set; } // ✅ Eklendi - PuantajService'de kullanılıyor
    }
}
