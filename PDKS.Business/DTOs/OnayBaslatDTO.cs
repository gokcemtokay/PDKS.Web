using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDKS.Business.DTOs
{
    public class OnayBaslatDTO
    {
        public string ModulTipi { get; set; } // "Izin", "Avans", "Masraf"
        public int ReferansId { get; set; } // İzin ID, Avans ID vs.
        public int TalepEdenKullaniciId { get; set; }
    }
}
