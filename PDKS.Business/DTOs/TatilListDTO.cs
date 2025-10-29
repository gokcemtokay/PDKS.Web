using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDKS.Business.DTOs
{
    public class TatilListDTO
    {
        
        public int SirketId { get; set; }
public int Id { get; set; }
        public string Ad { get; set; }
        public DateTime Tarih { get; set; }
        public string? Aciklama { get; set; }
    }
}