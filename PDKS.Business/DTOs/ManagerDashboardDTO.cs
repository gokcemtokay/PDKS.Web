using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDKS.Business.DTOs
{
    public class ManagerDashboardDTO
    {
        public EkipOzetiDTO EkipOzeti { get; set; }
        public List<EkipUyesiDTO> EkipUyeleri { get; set; }
        public List<BekleyenTalepDTO> BekleyenTalepler { get; set; }
        public EkipIzinTakvimiDTO IzinTakvimi { get; set; }
        public ButceKullanimiDTO ButceKullanimi { get; set; }
    }
}
