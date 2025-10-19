using PDKS.Data.Entities;
using PDKS.Data.Repositories;
using System;
using System.Security.Cryptography;
using System.Text;
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
            var kullanici = await _unitOfWork.Kullanicilar.FirstOrDefaultAsync(k => k.Email == email && k.Aktif);
            if (kullanici != null && VerifyPassword(kullanici.SifreHash, password))
            {
                kullanici.Rol = await _unitOfWork.Roller.GetByIdAsync(kullanici.RolId);
                return kullanici;
            }
            return null;
        }

        public string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        public bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            return HashPassword(providedPassword) == hashedPassword;
        }
    }
}