using System.Security.Cryptography;
using System.Text;
using PDKS.Data.Entities;
using PDKS.Data.Repositories;

namespace PDKS.Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuthService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<(bool Success, Kullanici User, string Message)> LoginAsync(string email, string password)
        {
            var kullanici = await _unitOfWork.Kullanicilar.FirstOrDefaultAsync(k => k.Email == email);

            if (kullanici == null)
                return (false, null, "Kullanıcı bulunamadı");

            if (!kullanici.Aktif)
                return (false, null, "Kullanıcı hesabı aktif değil");

            if (!VerifyPassword(password, kullanici.SifreHash))
                return (false, null, "Şifre hatalı");

            // Update last login
            kullanici.SonGirisTarihi = DateTime.UtcNow;
            _unitOfWork.Kullanicilar.Update(kullanici);
            await _unitOfWork.SaveChangesAsync();

            return (true, kullanici, "Giriş başarılı");
        }

        public async Task<bool> RegisterAsync(int personelId, string email, string password, int rolId)
        {
            // Check if user already exists
            var exists = await _unitOfWork.Kullanicilar.AnyAsync(k => k.Email == email);
            if (exists)
                return false;

            var kullanici = new Kullanici
            {
                PersonelId = personelId,
                Email = email,
                SifreHash = HashPassword(password),
                RolId = rolId,
                Aktif = true,
                OlusturmaTarihi = DateTime.UtcNow
            };

            await _unitOfWork.Kullanicilar.AddAsync(kullanici);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ChangePasswordAsync(int kullaniciId, string oldPassword, string newPassword)
        {
            var kullanici = await _unitOfWork.Kullanicilar.GetByIdAsync(kullaniciId);
            if (kullanici == null)
                return false;

            if (!VerifyPassword(oldPassword, kullanici.SifreHash))
                return false;

            kullanici.SifreHash = HashPassword(newPassword);
            _unitOfWork.Kullanicilar.Update(kullanici);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }

        public bool VerifyPassword(string password, string hash)
        {
            var passwordHash = HashPassword(password);
            return passwordHash == hash;
        }
    }
}
