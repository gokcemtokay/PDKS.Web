// PDKS.Business/DTOs/DepartmanUpdateDTO.cs
using System.ComponentModel.DataAnnotations;

namespace PDKS.Business.DTOs
{
    public class DepartmanUpdateDTO
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Şirket seçimi zorunludur")]
        public int SirketId { get; set; }

        [Required(ErrorMessage = "Departman adı zorunludur")]
        [StringLength(100, ErrorMessage = "Departman adı en fazla 100 karakter olabilir")]
        public string DepartmanAdi { get; set; }

        [StringLength(50, ErrorMessage = "Kod en fazla 50 karakter olabilir")]
        public string Kod { get; set; }

        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir")]
        public string Aciklama { get; set; }

        public int? UstDepartmanId { get; set; }

        public bool Durum { get; set; }
    }
}