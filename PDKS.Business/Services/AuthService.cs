using DocumentFormat.OpenXml.Math;
using PDKS.Data.Entities;
using PDKS.Data.Repositories;
using System.Threading.Tasks;

namespace PDKS.Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuthService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Kullanici> ValidateUserAsync(string email, string password)
        {
            var kullanici = await _unitOfWork.Kullanicilar
                .FirstOrDefaultAsync(k => k.Email == email && k.Aktif);

            if (kullanici != null && VerifyPassword(kullanici.SifreHash, password))
            {
                kullanici.Rol = await _unitOfWork.Roller.GetByIdAsync(kullanici.RolId);
                return kullanici;
            }

            return null;
        }

        public string HashPassword(string password)
        {
            // BCrypt kullan
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            // BCrypt ile doğrula
            try
            {
                return BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword);
            }
            catch
            {
                // Hash geçersizse false dön
                return false;
            }
        }
    }
}