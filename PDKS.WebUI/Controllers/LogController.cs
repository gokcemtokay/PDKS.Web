using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PDKS.Data.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PDKS.WebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
#if DEBUG
    [Route("api/[controller]")] // ⬅️ Development: /api/auth/login
#else
[Route("[controller]")] // ⬅️ Production: /auth/login (IIS /api ekler)
#endif
    public class LogController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public LogController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Yardımcı metot: JWT token'dan aktif şirket ID'sini alır.
        private int GetCurrentSirketId()
        {
            var sirketIdClaim = User.Claims.FirstOrDefault(c => c.Type == "sirketId");
            if (sirketIdClaim != null && int.TryParse(sirketIdClaim.Value, out int sirketId))
            {
                return sirketId;
            }
            throw new UnauthorizedAccessException("Yetkilendirme token'ında şirket ID'si bulunamadı.");
        }


        // GET: api/Log
        [HttpGet]
        public async Task<IActionResult> GetLogs()
        {
            try
            {
                var loglar = await _unitOfWork.Loglar.GetAllAsync();
                var result = loglar
                    .OrderByDescending(l => l.Tarih)
                    .Select(l => new
                    {
                        l.Id,
                        KullaniciAdi = l.Kullanici != null ? l.Kullanici.Personel.AdSoyad : "Sistem",
                        l.Tarih,
                        l.Islem,     // Artık doğru alan adını kullanıyoruz
                        l.Aciklama,  // Artık doğru alan adını kullanıyoruz
                        l.IpAdresi,  // Artık doğru alan adını kullanıyoruz
                        l.LogLevel   // Artık doğru alan adını kullanıyoruz
                    });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}