// PDKS.Business/DTOs/DepartmanDetailDTO.cs
namespace PDKS.Business.DTOs
{
    public class DepartmanDetailDTO
    {
        public int Id { get; set; }
        public int SirketId { get; set; }
        public string SirketAdi { get; set; }
        public string DepartmanAdi { get; set; }
        public string Kod { get; set; }
        public string Aciklama { get; set; }
        public int? UstDepartmanId { get; set; }
        public string UstDepartmanAdi { get; set; }
        public bool Durum { get; set; }
        public int PersonelSayisi { get; set; }
        public int AltDepartmanSayisi { get; set; }
        public DateTime KayitTarihi { get; set; }
        public DateTime? OlusturmaTarihi { get; set; }
        public DateTime? GuncellemeTarihi { get; set; }
    }
}