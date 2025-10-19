// PDKS.Business/DTOs/SirketListDTO.cs - TAMAMI
namespace PDKS.Business.DTOs
{
    public class SirketListDTO
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
        public bool Aktif { get; set; }
        public bool AnaSirket { get; set; }
        public string AnaSirketAdi { get; set; }     
        public int PersonelSayisi { get; set; }
        public DateTime? KurulusTarihi { get; set; } 
        public DateTime OlusturmaTarihi { get; set; }
    }
}