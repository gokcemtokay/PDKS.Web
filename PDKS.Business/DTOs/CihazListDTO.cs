using System;

namespace PDKS.Business.DTOs
{
    public class CihazListDTO
    {
        public int SirketId { get; set; }
        public int Id { get; set; }
        public string CihazAdi { get; set; }
        public string CihazTipi { get; set; }  // ← YENİ ALAN
        public string IPAdres { get; set; }
        public int? Port { get; set; }  // ← YENİ ALAN
        public string Lokasyon { get; set; }
        public bool Durum { get; set; }
        public string DurumText => Durum ? "Aktif" : "Pasif";
        public DateTime? SonBaglantiZamani { get; set; }
        public int BugunkuOkumaSayisi { get; set; }
    }
}