using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDKS.Business.DTOs
{
    public class DepartmanMaliyetDTO
    {
        
        public int SirketId { get; set; }
public string DepartmanAdi { get; set; }
        public decimal Maliyet { get; set; }
        public int PersonelSayisi { get; set; }
        public decimal KisiBasinaMaliyet { get; set; }
    }
}
