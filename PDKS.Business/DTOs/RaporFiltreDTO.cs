namespace PDKS.Business.DTOs
{
    // Rapor Filtre DTO
    public class RaporFiltreDTO
    {
        public int? PersonelId { get; set; }
        
        public int SirketId { get; set; }
public string Departman { get; set; }
        public DateTime BaslangicTarihi { get; set; } = DateTime.Today.AddMonths(-1);
        public DateTime BitisTarihi { get; set; } = DateTime.Today;
        public int? Yil { get; set; } = DateTime.Today.Year;
        public int? Ay { get; set; } = DateTime.Today.Month;
    }
}
