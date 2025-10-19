// PDKS.Business/DTOs/SirketDetailDTO.cs - TAMAMI
namespace PDKS.Business.DTOs
{
    public class SirketDetailDTO
    {
        public int Id { get; set; }
        public string Unvan { get; set; }
        public string TicariUnvan { get; set; }
        public string VergiNo { get; set; }
        public string VergiDairesi { get; set; }
        public string Telefon { get; set; }
        public string Email { get; set; }
        public string Adres { get; set; }
        public string Il { get; set; }
        public string Ilce { get; set; }
        public string PostaKodu { get; set; }
        public string Website { get; set; }
        public string LogoUrl { get; set; }          // ✅ Zaten var
        public DateTime? KurulusTarihi { get; set; }
        public bool Aktif { get; set; }
        public string ParaBirimi { get; set; }
        public string Notlar { get; set; }
        public bool AnaSirket { get; set; }
        public int? AnaSirketId { get; set; }
        public string AnaSirketAdi { get; set; }     // ✅ Eklendi
        public int PersonelSayisi { get; set; }      // ✅ Eklendi
        public int DepartmanSayisi { get; set; }     // ✅ Eklendi
        public int BagliSirketSayisi { get; set; }
        public DateTime? OlusturmaTarihi { get; set; }// ✅ Eklendi
        public DateTime? GuncellemeTarihi { get; set; } // ✅ Eklendi
    }
}