using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDKS.Business.DTOs
{
    public class ExecutiveSummaryDTO
    {
        
        public int SirketId { get; set; }
public int ToplamPersonel { get; set; }
        public decimal ToplamMaliyet { get; set; }
        public double PersonelMemnuniyeti { get; set; }
        public double Verimlilik { get; set; }
        public double PersonelDevri { get; set; }
    }
}
