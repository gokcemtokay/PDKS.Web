using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDKS.Data.Entities
{
    public class OnayDetay
    {
        public int Id { get; set; }
        public int OnayKaydiId { get; set; }
        public int OnayAdimiId { get; set; }
        public int AdimSira { get; set; }
        public int? OnaylayanKullaniciId { get; set; }
        public string Durum { get; set; }
        public DateTime? OnayTarihi { get; set; }
        public string Aciklama { get; set; }
        public DateTime OlusturmaTarihi { get; set; }
        public DateTime? GuncellemeTarihi { get; set; }

        public virtual OnayKaydi OnayKaydi { get; set; }
        public virtual OnayAdimi OnayAdimi { get; set; }
        public virtual Kullanici OnaylayanKullanici { get; set; }
    }
}
