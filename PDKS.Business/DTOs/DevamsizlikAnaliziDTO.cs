using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDKS.Business.DTOs
{
    public class DevamsizlikAnaliziDTO
    {
        public int ToplamDevamsizGun { get; set; }
        public double DevamsizlikOrani { get; set; }
        public List<DepartmanDevamsizlikDTO> DepartmanBazinda { get; set; }
    }
}
