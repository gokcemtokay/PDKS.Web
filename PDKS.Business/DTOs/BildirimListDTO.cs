using System;

namespace PDKS.Business.DTOs
{
    public class BildirimListDTO
    {
        public int Id { get; set; }
        public int KullaniciId { get; set; }
        public string Baslik { get; set; }
        public string? Mesaj { get; set; }
        public string? Tip { get; set; }
        public bool Okundu { get; set; }
        public DateTime OlusturmaTarihi { get; set; }
        public DateTime? OkunmaTarihi { get; set; }
        public string? Link { get; set; }
        public string? ReferansTip { get; set; }
        public int? ReferansId { get; set; }
    }
}
