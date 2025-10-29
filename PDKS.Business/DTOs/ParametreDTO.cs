using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDKS.Business.DTOs
{
    public class ParametreDTO
    {
        
        public int SirketId { get; set; }
public int Id { get; set; }
        public string Ad { get; set; }
        public string Deger { get; set; }
        public string? Birim { get; set; }
        public string? Aciklama { get; set; }
        public string? Kategori { get; set; }
    }
}
