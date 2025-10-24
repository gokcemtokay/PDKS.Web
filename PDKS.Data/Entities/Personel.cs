using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("Personeller")]
    public class Personel
    {
        [Key]
        public int Id { get; set; }

        // ⭐ YENİ: Şirket bağlantısı
        [Required]
        [Column("sirket_id")]
        public int SirketId { get; set; }

        [Required]
        [StringLength(100)]
        public string AdSoyad { get; set; }

        [Required]
        [StringLength(20)]
        public string SicilNo { get; set; }

        [Required]
        [StringLength(11)]
        public string TcKimlikNo { get; set; }

        [StringLength(500)]
        public string? ProfilResmi { get; set; } // Profil fotoğrafı yolu

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(15)]
        public string? Telefon { get; set; }

        [StringLength(500)]
        public string? Adres { get; set; }

        public DateTime DogumTarihi { get; set; }

        [StringLength(10)]
        public string? Cinsiyet { get; set; }

        [StringLength(50)]
        public string? KanGrubu { get; set; }

        public DateTime GirisTarihi { get; set; }

        public DateTime? CikisTarihi { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Maas { get; set; }

        [StringLength(100)]
        public string? Unvan { get; set; }

        [StringLength(100)]
        public string? Gorev { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? AvansLimiti { get; set; }

        public bool Durum { get; set; } = true;

        // Foreign Keys
        public int? DepartmanId { get; set; }
        public int? VardiyaId { get; set; }

        // 🆕 Yeni Foreign Keys
        public int? YoneticiPersonelId { get; set; } // Direkt amir/yönetici
        public int? IkinciAmirPersonelId { get; set; } // İkincil amir (Matrix organizasyon için)

        [StringLength(50)]
        public string? CalismaModeli { get; set; } // Tam Zamanlı, Yarı Zamanlı, Uzaktan, Hibrit

        [StringLength(100)]
        public string? CalismaLokasyonu { get; set; } // Hangi ofis/şube

        [StringLength(500)]
        public string? Notlar { get; set; }

        public DateTime KayitTarihi { get; set; } = DateTime.UtcNow;
        public DateTime? OlusturmaTarihi { get; set; } = DateTime.UtcNow;
        public DateTime? GuncellemeTarihi { get; set; }

        // ============== NAVIGATION PROPERTIES ==============

        // Mevcut Navigation Properties
        [ForeignKey("SirketId")]
        public virtual Sirket Sirket { get; set; }

        [ForeignKey("DepartmanId")]
        public virtual Departman? Departman { get; set; }

        [ForeignKey("VardiyaId")]
        public virtual Vardiya? Vardiya { get; set; }

        // 🆕 Yönetici Bağlantıları
        [ForeignKey("YoneticiPersonelId")]
        public virtual Personel? YoneticiPersonel { get; set; }

        [ForeignKey("IkinciAmirPersonelId")]
        public virtual Personel? IkinciAmirPersonel { get; set; }

        public virtual Kullanici? Kullanici { get; set; }

        // Mevcut Collections
        public virtual ICollection<GirisCikis> GirisCikislar { get; set; } = new List<GirisCikis>();
        public virtual ICollection<Izin> Izinler { get; set; } = new List<Izin>();
        public virtual ICollection<Mesai> Mesailer { get; set; } = new List<Mesai>();
        public virtual ICollection<Avans> Avanslar { get; set; } = new List<Avans>();
        public virtual ICollection<Prim> Primler { get; set; } = new List<Prim>();

        // 🆕 YENİ Collections - Özlük Bilgileri
        public virtual ICollection<PersonelAile> AileBilgileri { get; set; } = new List<PersonelAile>();
        public virtual ICollection<PersonelAcilDurum> AcilDurumBilgileri { get; set; } = new List<PersonelAcilDurum>();
        public virtual PersonelSaglik? SaglikBilgisi { get; set; } // One-to-One
        public virtual ICollection<PersonelEgitim> EgitimGecmisi { get; set; } = new List<PersonelEgitim>();
        public virtual ICollection<PersonelIsDeneyimi> IsDeneyimleri { get; set; } = new List<PersonelIsDeneyimi>();
        public virtual ICollection<PersonelDil> DilBecerileri { get; set; } = new List<PersonelDil>();
        public virtual ICollection<PersonelSertifika> Sertifikalar { get; set; } = new List<PersonelSertifika>();
        public virtual ICollection<PersonelPerformans> PerformansKayitlari { get; set; } = new List<PersonelPerformans>();
        public virtual ICollection<PersonelDisiplin> DisiplinKayitlari { get; set; } = new List<PersonelDisiplin>();
        public virtual ICollection<PersonelTerfi> TerfiGecmisi { get; set; } = new List<PersonelTerfi>();
        public virtual ICollection<PersonelUcretDegisiklik> UcretDegisiklikleri { get; set; } = new List<PersonelUcretDegisiklik>();
        public virtual ICollection<PersonelReferans> Referanslar { get; set; } = new List<PersonelReferans>();
        public virtual ICollection<PersonelZimmet> ZimmetliEsyalar { get; set; } = new List<PersonelZimmet>();
        public virtual ICollection<PersonelYetkinlik> Yetkinlikler { get; set; } = new List<PersonelYetkinlik>();
        public virtual ICollection<PersonelEgitimKayit> EgitimKayitlari { get; set; } = new List<PersonelEgitimKayit>();
        public virtual PersonelMaliBilgi? MaliBilgi { get; set; } // One-to-One
        public virtual PersonelEkBilgi? EkBilgi { get; set; } // One-to-One

        // Self-referencing for Yönetici-Çalışan ilişkisi
        public virtual ICollection<Personel> AltCalisanlar { get; set; } = new List<Personel>();
        public virtual ICollection<Personel> IkinciAmirOlduguCalisanlar { get; set; } = new List<Personel>();
    }
}
