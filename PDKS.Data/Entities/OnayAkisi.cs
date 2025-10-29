// PDKS.Data/Entities/OnayAkisi.cs - YENİ

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PDKS.Data.Entities
{
    public class OnayAkisi
    {
        public int Id { get; set; }
        [Required]
        public int SirketId { get; set; }
        public string AkisAdi { get; set; }
        public string ModulTipi { get; set; }
        public string Aciklama { get; set; }
        public bool Aktif { get; set; }
        public DateTime OlusturmaTarihi { get; set; }
        public DateTime? GuncellemeTarihi { get; set; }

        // Navigation
        public virtual Sirket Sirket { get; set; }
        public virtual ICollection<OnayAdimi> OnayAdimlari { get; set; }
    }
}