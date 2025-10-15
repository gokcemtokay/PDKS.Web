namespace PDKS.Business.DTOs
{
    // Aylık Devam Çizelgesi DTO
    public class AylikDevamCizelgesiDTO
    {
        public string PersonelAdi { get; set; }
        public string SicilNo { get; set; }
        public string Departman { get; set; }
        public string Donem { get; set; }
        public List<DevamGunDTO> Gunler { get; set; }
        public int ToplamCalismaGunu { get; set; }
        public int ToplamDevamsizGun { get; set; }
        public int ToplamIzinGun { get; set; }
        public int ToplamCalismaSaati { get; set; }
    }
}
