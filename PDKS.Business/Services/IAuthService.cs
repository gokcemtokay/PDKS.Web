using PDKS.Data.Entities;

namespace PDKS.Business.Services
{
    // Authentication Service
    public interface IAuthService
    {
        Task<(bool Success, Kullanici User, string Message)> LoginAsync(string email, string password);
        Task<bool> RegisterAsync(int personelId, string email, string password, int rolId);
        Task<bool> ChangePasswordAsync(int kullaniciId, string oldPassword, string newPassword);
        string HashPassword(string password);
        bool VerifyPassword(string password, string hash);
    }
}
