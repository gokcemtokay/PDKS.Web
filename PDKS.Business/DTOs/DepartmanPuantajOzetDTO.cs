using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDKS.Business.DTOs
{
    // Departman Puantaj Özet DTO
    public class DepartmanPuantajOzetDTO
    {
        public int DepartmanId { get; set; }
        public string DepartmanAdi { get; set; }
        public int PersonelSayisi { get; set; }
        public decimal ToplamCalismaSaati { get; set; }
        public decimal ToplamFazlaMesai { get; set; }
        public int ToplamDevamsizlik { get; set; }
        public decimal OrtalamaCalismaOrani { get; set; }
    }
}
