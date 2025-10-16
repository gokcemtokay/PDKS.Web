using System.ComponentModel.DataAnnotations;

namespace PDKS.Business.DTOs
{
    public class DepartmanCreateDTO
    {
        [Required(ErrorMessage = "Departman adı zorunludur")]
        [StringLength(100)]
        public string Ad { get; set; }

        [StringLength(50)]
        public string? Kod { get; set; }

        [StringLength(500)]
        public string? Aciklama { get; set; }

        public int? UstDepartmanId { get; set; }

        public bool Durum { get; set; } = true;
    }
}