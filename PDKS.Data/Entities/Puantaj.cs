using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("Puantajlar")]
    public class Puantaj
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SirketId { get; set; }

        [Required]
        public int PersonelId { get; set; }

        [Required]
        public int Yil { get; set; }

        [Required]
        public int Ay { get; set; }

        // Çalışma İstatistikleri
        public int ToplamCalismaSuresi { get; set; } // dakika
        public int NormalMesaiSuresi { get; set; } // dakika
        public int FazlaMesaiSuresi { get; set; } // dakika
        public int GeceMesaiSuresi { get; set; } // dakika
        public int HaftaTatiliCalismaSuresi { get; set; } // dakika
        public int ResmiTatilCalismaSuresi { get; set; } // dakika

        // Gün Sayıları
        public int ToplamCalisilanGun { get; set; }
        public int GecKalmaGunSayisi { get; set; }
        public int ErkenCikisGunSayisi { get; set; }
        public int DevamsizlikGunSayisi { get; set; }
        public int IzinGunSayisi { get; set; }
        public int HastaTatiliGunSayisi { get; set; }
        public int MazeretliIzinGunSayisi { get; set; }
        public int UcretsizIzinGunSayisi { get; set; }
        public int HaftaTatiliGunSayisi { get; set; }
        public int ResmiTatilGunSayisi { get; set; }

        // Süre Detayları
        public int ToplamGecKalmaSuresi { get; set; } // dakika
        public int ToplamErkenCikisSuresi { get; set; } // dakika
        public int ToplamEksikCalismaSuresi { get; set; } // dakika

        // Durum
        public string Durum { get; set; } // Taslak, Onaylandı, Kapalı
        public bool Onaylandi { get; set; }
        public int? OnaylayanKullaniciId { get; set; }
        public DateTime? OnayTarihi { get; set; }

        public string Notlar { get; set; }
        public DateTime OlusturmaTarihi { get; set; }
        public DateTime? GuncellemeTarihi { get; set; }

        // Navigation Properties
        [ForeignKey("SirketId")]
        public virtual Sirket Sirket { get; set; }

        [ForeignKey("PersonelId")]
        public virtual Personel Personel { get; set; }

        [ForeignKey("OnaylayanKullaniciId")]
        public virtual Kullanici OnaylayanKullanici { get; set; }

        public virtual ICollection<PuantajDetay> PuantajDetaylari { get; set; }
    }
}
