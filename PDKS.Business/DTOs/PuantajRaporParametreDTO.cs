using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDKS.Business.DTOs
{
    // Puantaj Rapor Parametre DTO
    public class PuantajRaporParametreDTO
    {
        public int? SirketId { get; set; }
        public int BaslangicYil { get; set; }
        public int BaslangicAy { get; set; }
        public int BitisYil { get; set; }
        public int BitisAy { get; set; }
        public int? DepartmanId { get; set; }
        public int? PersonelId { get; set; }
        public string RaporTuru { get; set; } // Ozet, Detayli, FazlaMesai, Devamsizlik
    }
}
