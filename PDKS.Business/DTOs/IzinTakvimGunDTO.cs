using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDKS.Business.DTOs
{
    public class IzinTakvimGunDTO
    {
        public DateTime Tarih { get; set; }
        public List<IzinliPersonelDTO> IzinliPersoneller { get; set; }
    }
}
