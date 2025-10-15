using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PDKS.Data.Entities
{
    [Table("cihazlar")]
    public class Cihaz
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("cihaz_adi")]
        public string CihazAdi { get; set; }

        [MaxLength(50)]
        [Column("ip_adres")]
        public string IPAdres { get; set; }

        [MaxLength(200)]
        [Column("lokasyon")]
        public string Lokasyon { get; set; }

        [Column("durum")]
        public bool Durum { get; set; } = true;

        [Column("son_baglanti_zamani")]
        public DateTime? SonBaglantiZamani { get; set; }

        [Column("olusturma_tarihi")]
        public DateTime OlusturmaTarihi { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public virtual ICollection<CihazLog> CihazLoglari { get; set; }
        public virtual ICollection<GirisCikis> GirisCikislar { get; set; }
    }
}
