using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDKS.Data.Entities
{
    public class OnayAdimi
    {
        public int Id { get; set; }
        public int OnayAkisiId { get; set; }
        public int Sira { get; set; }
        public string AdimAdi { get; set; }

        public int? OnaylayanRolId { get; set; }
        public int? OnaylayanKullaniciId { get; set; }
        public int? OnaylayanDepartmanId { get; set; }
        public string OnaylayanTipi { get; set; }

        public bool Zorunlu { get; set; }
        public int? TimeoutGun { get; set; }
        public int? EscalateKullaniciId { get; set; }

        public DateTime OlusturmaTarihi { get; set; }
        public DateTime? GuncellemeTarihi { get; set; }

        public virtual OnayAkisi OnayAkisi { get; set; }
        public virtual Rol OnaylayanRol { get; set; }
        public virtual Kullanici OnaylayanKullanici { get; set; }
        public virtual Departman OnaylayanDepartman { get; set; }
    }
}
