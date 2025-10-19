using System;

namespace PDKS.Business.DTOs
{
    public class IzinDetailDTO
    {
        public int Id { get; set; }
        public int PersonelId { get; set; }
        public string PersonelAdi { get; set; }
        public string IzinTipi { get; set; }
        public DateTime BaslangicTarihi { get; set; }
        public DateTime BitisTarihi { get; set; }
        public int GunSayisi { get; set; }
        public string Aciklama { get; set; }
        public string OnayDurumu { get; set; }
        public string RedNedeni { get; set; }
        public DateTime? OnayTarihi { get; set; }
        public int? OnaylayanKullaniciId { get; set; }
        public string OnaylayanAdi { get; set; }
        public DateTime OlusturmaTarihi { get; set; }

        // YENİ EKLENEN VE HATALARI GİDERECEK ALANLAR
        public string PersonelSicilNo { get; set; }
        public DateTime TalepTarihi { get; set; } // TalepTarihi, OlusturmaTarihi ile aynı olacak
        public string OnaylayanKullaniciAdi { get; set; } // OnaylayanKullaniciAdi, OnaylayanAdi ile aynı olacak
    }
}