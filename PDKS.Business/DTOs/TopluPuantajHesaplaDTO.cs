using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDKS.Business.DTOs
{
    // Toplu Puantaj Hesaplama DTO
    public class TopluPuantajHesaplaDTO
    {
        public int SirketId { get; set; }
        public int Yil { get; set; }
        public int Ay { get; set; }
        public List<int> PersonelIdler { get; set; }
        public bool TumPersonel { get; set; }
        public int? DepartmanId { get; set; }
    }
}
