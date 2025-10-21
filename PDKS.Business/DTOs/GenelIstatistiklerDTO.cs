using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDKS.Business.DTOs
{
    public class GenelIstatistiklerDTO
    {
        public int ToplamPersonel { get; set; }
        public int AktifPersonel { get; set; }
        public int BuAyIseBaslayan { get; set; }
        public int BuAyCikanPersonel { get; set; }
        public double PersonelDevri { get; set; } // Turnover rate
        public double OrtalamaKidem { get; set; }
    }
}
