using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDKS.Business.DTOs
{
    // Puantaj Rapor Filtre DTO
    public class PuantajRaporFiltreDTO
    {
        public int SirketId { get; set; }
        public int Yil { get; set; }
        public int Ay { get; set; }
        public int? DepartmanId { get; set; }
        public int? PersonelId { get; set; }
        public bool? SadeceOnaylanan { get; set; }
        public string RaporTipi { get; set; } // Ozet, Detayli, FazlaMesai, Devamsizlik, vb.
    }
}
