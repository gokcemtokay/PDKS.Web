using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDKS.Business.DTOs
{
    public class IKDashboardDTO
    {
        public GenelIstatistiklerDTO GenelIstatistikler { get; set; }
        public DevamsizlikAnaliziDTO DevamsizlikAnalizi { get; set; }
        public MaliyetAnaliziDTO MaliyetAnalizi { get; set; }
        public PersonelDagilimDTO PersonelDagilimi { get; set; }
        public IseAlimDTO IseAlim { get; set; }
    }
}
