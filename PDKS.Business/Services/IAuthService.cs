using PDKS.Data.Entities;
using System.Threading.Tasks;

namespace PDKS.Business.Services
{
    public interface IAuthService
    {
        Task<Kullanici> ValidateUserAsync(string email, string password);
        string HashPassword(string password);
        bool VerifyPassword(string hashedPassword, string providedPassword);
    }
}