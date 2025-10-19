using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDKS.Business.DTOs
{
    public class SirketListDTO
    {
        public int Id { get; set; }
        public string Unvan { get; set; }
        public string VergiNo { get; set; }
        public string Telefon { get; set; }
        public string Email { get; set; }
        public string Il { get; set; }
        public bool Aktif { get; set; }
        public int PersonelSayisi { get; set; }
        public bool AnaSirket { get; set; }
        public string AnaSirketUnvan { get; set; }
    }
}
