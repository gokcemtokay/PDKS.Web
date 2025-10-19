using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDKS.Business.DTOs
{
    public class KonsolideRaporDTO
    {
        public int SirketId { get; set; }
        public string SirketAdi { get; set; }
        public int BagliSirketSayisi { get; set; }
        public int ToplamPersonelSayisi { get; set; }
        public int ToplamDepartmanSayisi { get; set; }
        public int TransferSayisi { get; set; }
        public string Donem { get; set; }
        public decimal ToplamMaas { get; set; }
        public int AktifPersonelSayisi { get; set; }
        public int PasifPersonelSayisi { get; set; }
        public List<KonsolideSirketDetayDTO> BagliSirketDetaylari { get; set; } = new List<KonsolideSirketDetayDTO>();
    }
}
