namespace PDKS.Business.DTOs
{
    // Prim DTOs
    public class PrimListDTO
    {
        
        public int SirketId { get; set; }
public int Id { get; set; } // ✅ Eklendi
        public int PersonelId { get; set; }
        public string PersonelAdi { get; set; }
        public string SicilNo { get; set; }
        public string Departman { get; set; }
        public decimal PerformansPrimi { get; set; }
        public decimal SatisPrimi { get; set; }
        public decimal UretimPrimi { get; set; }
        public decimal DigerPrimler { get; set; }
        public decimal ToplamPrim => PerformansPrimi + SatisPrimi + UretimPrimi + DigerPrimler;
        public decimal Tutar { get; set; }
        public int Yil { get; set; }
        public int Ay { get; set; }
        public string Donem { get; set; } // ✅ set; eklendi
        public string PrimTipi { get; set; } // ✅ Eklendi
        public string Aciklama { get; set; } // ✅ Eklendi
        public DateTime VerilmeTarihi { get; set; }
    }
}
