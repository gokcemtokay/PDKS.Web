using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDKS.Business.DTOs
{
    public class OnayDurumuDTO
    {
        public int OnayKaydiId { get; set; }
        public string ModulTipi { get; set; }
        public int ReferansId { get; set; }
        public string TalepEdenKisi { get; set; }
        public DateTime TalepTarihi { get; set; }
        public string GenelDurum { get; set; }
        public int MevcutAdimSira { get; set; }
        public string MevcutAdimAdi { get; set; }
        public List<OnayDetayDTO> OnayDetaylari { get; set; }
    }
}
