using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDKS.Business.DTOs
{
    public class ExecutiveDashboardDTO
    {
        public ExecutiveSummaryDTO Summary { get; set; }
        public List<DepartmanKarsilastirmaDTO> DepartmanKarsilastirma { get; set; }
        public FinansalOzetDTO FinansalOzet { get; set; }
        public List<TrendDTO> Trendler { get; set; }
        public List<KPIDTO> KPIlar { get; set; }
    }
}
