using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDKS.Business.DTOs
{
    public class OnayDetayDTO
    {
        public int AdimSira { get; set; }
        public string AdimAdi { get; set; }
        public string OnaylayanKisi { get; set; }
        public string Durum { get; set; }
        public DateTime? OnayTarihi { get; set; }
        public string Aciklama { get; set; }
    }
}
