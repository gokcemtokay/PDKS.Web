using System.ComponentModel.DataAnnotations;

namespace PDKS.Business.DTOs
{
    public class LoginDto
    {
        [Required(ErrorMessage = "E-posta adresi gereklidir.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        
        public int SirketId { get; set; }
public string Email { get; set; }

        [Required(ErrorMessage = "Şifre gereklidir.")]
        public string Password { get; set; }
    }
}