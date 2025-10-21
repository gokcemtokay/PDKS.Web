using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDKS.Business.DTOs
{
    public class OnayAkisiDTO
    {
        public int? Id { get; set; }
        public int SirketId { get; set; }
        public string AkisAdi { get; set; }
        public string ModulTipi { get; set; } // "Izin", "Avans", "Masraf", "Seyahat"
        public string Aciklama { get; set; }
        public bool Aktif { get; set; }
        public List<OnayAdimiDTO> OnayAdimlari { get; set; } = new List<OnayAdimiDTO>();
    }
}
