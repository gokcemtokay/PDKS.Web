using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDKS.Business.DTOs
{
    public class BugunkunDurumDTO
    {
        
        public int SirketId { get; set; }
public int ToplamPersonel { get; set; }
        public int BugunkuGiris { get; set; }
        public int AktifPersonel { get; set; }
        public int IzinliPersonel { get; set; }
        public int RaporluPersonel { get; set; }
        public int GecKalanPersonel { get; set; }
        public int DevamsizPersonel { get; set; }
        public double GirisCikisOrani { get; set; }
    }
}
