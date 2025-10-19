using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDKS.Business.DTOs
{
    public class KonsolideRaporDTO
    {
        public string SirketUnvan { get; set; }
        public int ToplamPersonel { get; set; }
        public int AktifPersonel { get; set; }
        public int DevamsizPersonel { get; set; }
        public int IzinliPersonel { get; set; }
        public decimal ToplamMaasBordrosu { get; set; }
        public int ToplamFazlaMesaiSaati { get; set; }
        public decimal ToplamAvans { get; set; }
        public decimal ToplamPrim { get; set; }
    }
}
