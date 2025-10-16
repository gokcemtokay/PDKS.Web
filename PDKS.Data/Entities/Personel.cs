using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("Personeller")]
    public class Personel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string AdSoyad { get; set; }

        [Required]
        [StringLength(20)]
        public string SicilNo { get; set; }

        [Required]
        [StringLength(11)]
        public string TcKimlikNo { get; set; }

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

        [StringLength(500)]
        public string? Notlar { get; set; }

        public DateTime KayitTarihi { get; set; } = DateTime.UtcNow;

        public DateTime? OlusturmaTarihi { get; set; } = DateTime.UtcNow;

        public DateTime? GuncellemeTarihi { get; set; }

        // Navigation Properties
        [ForeignKey("DepartmanId")]
        public Departman? Departman { get; set; }

        [ForeignKey("VardiyaId")]
        public Vardiya? Vardiya { get; set; }

        public Kullanici? Kullanici { get; set; }

        public ICollection<GirisCikis> GirisCikislar { get; set; } = new List<GirisCikis>();
        public ICollection<Izin> Izinler { get; set; } = new List<Izin>();
        public ICollection<Mesai> Mesailer { get; set; } = new List<Mesai>();
        public ICollection<Avans> Avanslar { get; set; } = new List<Avans>();
        public ICollection<Prim> Primler { get; set; } = new List<Prim>();
    }
}