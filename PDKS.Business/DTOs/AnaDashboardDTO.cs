using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDKS.Business.DTOs
{
    public class AnaDashboardDTO
    {
        public BugunkunDurumDTO BugunkunDurum { get; set; }
        public List<BekleyenOnayWidgetDTO> BekleyenOnaylar { get; set; }
        public List<SonAktiviteDTO> SonAktiviteler { get; set; }
        public List<DogumGunuDTO> DogumGunleri { get; set; }
        public List<YilDonumuDTO> YilDonumleri { get; set; }
        public List<DuyuruDTO> Duyurular { get; set; }
    }
}
