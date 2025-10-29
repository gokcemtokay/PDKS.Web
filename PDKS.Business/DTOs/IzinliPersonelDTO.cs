using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDKS.Business.DTOs
{
    public class IzinliPersonelDTO
    {
        
        public int SirketId { get; set; }
public int PersonelId { get; set; }
        public string AdSoyad { get; set; }
        public string IzinTipi { get; set; }
    }
}
