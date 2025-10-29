using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDKS.Business.DTOs
{
    public class DuyuruDTO
    {
        
        public int SirketId { get; set; }
public int Id { get; set; }
        public string Baslik { get; set; }
        public string Ozet { get; set; }
        public string Tip { get; set; } // "Bilgi", "Uyari", "Onemli"
        public DateTime Tarih { get; set; }
        public bool Okundu { get; set; }
    }
}
