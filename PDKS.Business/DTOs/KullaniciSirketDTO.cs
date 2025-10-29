using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDKS.Business.DTOs
{
    public class KullaniciSirketDTO
    {
        public int SirketId { get; set; }
        public string SirketAdi { get; set; } = string.Empty;
        public bool Varsayilan { get; set; }
        public bool Aktif { get; set; }
    }
}
