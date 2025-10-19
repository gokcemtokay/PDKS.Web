using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    public class CihazLog
    {
        public int Id { get; set; }
        public int CihazId { get; set; }
        public Cihaz Cihaz { get; set; }

        [Column("log_tarihi")]
        public DateTime Tarih { get; set; }

        [Column("log_mesaji")]
        public string Mesaj { get; set; }

        [Column("log_tipi")]
        public string Tip { get; set; }

        // --- Eski View'ların derleme hatasını gidermek için ---
        [NotMapped]
        public string Detay { get; set; }
        [NotMapped]
        public string Islem { get; set; }
        [NotMapped]
        public bool Basarili => Tip != "Hata";
    }
}