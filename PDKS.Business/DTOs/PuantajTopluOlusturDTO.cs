using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDKS.Business.DTOs
{
    // Toplu Puantaj Oluşturma DTO
    public class PuantajTopluOlusturDTO
    {
        public int SirketId { get; set; }
        public int Yil { get; set; }
        public int Ay { get; set; }
        public List<int> PersonelIdler { get; set; }
        public bool TumPersoneller { get; set; }
        public int? DepartmanId { get; set; }
    }
}
