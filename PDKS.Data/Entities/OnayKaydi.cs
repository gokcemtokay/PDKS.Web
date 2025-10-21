using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDKS.Data.Entities
{
    public class OnayKaydi
    {
        public int Id { get; set; }
        public int OnayAkisiId { get; set; }
        public string ModulTipi { get; set; }
        public int ReferansId { get; set; }
        public int TalepEdenKullaniciId { get; set; }
        public int MevcutAdimSira { get; set; }
        public string GenelDurum { get; set; }
        public DateTime TalepTarihi { get; set; }
        public DateTime? TamamlanmaTarihi { get; set; }
        public DateTime OlusturmaTarihi { get; set; }
        public DateTime? GuncellemeTarihi { get; set; }

        public virtual OnayAkisi OnayAkisi { get; set; }
        public virtual Kullanici TalepEdenKullanici { get; set; }
        public virtual ICollection<OnayDetay> OnayDetaylari { get; set; }
    }
}
