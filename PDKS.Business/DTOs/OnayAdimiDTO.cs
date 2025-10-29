using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDKS.Business.DTOs
{
    public class OnayAdimiDTO
    {
        public int? Id { get; set; }
        
        public int SirketId { get; set; }
public int Sira { get; set; }
        public string AdimAdi { get; set; }

        // Onaylayıcı belirleme
        public string OnaylayanTipi { get; set; } // "Rol", "Kullanici", "DepartmanMuduru", "UstYonetici"
        public int? OnaylayanRolId { get; set; }
        public int? OnaylayanKullaniciId { get; set; }
        public int? OnaylayanDepartmanId { get; set; }

        public bool Zorunlu { get; set; }
        public int? TimeoutGun { get; set; }
        public int? EscalateKullaniciId { get; set; }
    }
}
