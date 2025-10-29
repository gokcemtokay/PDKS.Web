using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDKS.Business.DTOs
{
    public class TrendDTO
    {
        
        public int SirketId { get; set; }
public string Baslik { get; set; }
        public List<TrendNoktaDTO> Veriler { get; set; }
    }
}
