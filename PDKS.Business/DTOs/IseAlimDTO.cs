using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDKS.Business.DTOs
{
    public class IseAlimDTO
    {
        
        public int SirketId { get; set; }
public int AktifIlanSayisi { get; set; }
        public int BaşvuruSayisi { get; set; }
        public int MulakataSagiri { get; set; }
        public int TeklifGonderilen { get; set; }
    }
}
