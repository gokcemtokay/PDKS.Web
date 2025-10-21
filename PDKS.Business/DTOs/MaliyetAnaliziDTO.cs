using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDKS.Business.DTOs
{
    public class MaliyetAnaliziDTO
    {
        public decimal ToplamMaasOdemesi { get; set; }
        public decimal ToplamAvans { get; set; }
        public decimal ToplamMasraf { get; set; }
        public decimal ToplamMaliyet { get; set; }
        public List<DepartmanMaliyetDTO> DepartmanBazinda { get; set; }
    }
}
