using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDKS.Business.DTOs
{
    public class SonAktiviteDTO
    {
        public string Tip { get; set; } // "GirisCikis", "Izin", "Avans", "Zimmet"
        public string Baslik { get; set; }
        public string Aciklama { get; set; }
        public string Kullanici { get; set; }
        public DateTime Tarih { get; set; }
        public string Icon { get; set; }
        public string Renk { get; set; }
    }
}
