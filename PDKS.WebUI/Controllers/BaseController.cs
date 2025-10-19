// PDKS.WebUI/Controllers/BaseController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PDKS.Data.Repositories;
using System.Security.Claims;

namespace PDKS.WebUI.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected int CurrentSirketId { get; private set; }
        protected int CurrentKullaniciId { get; private set; }

        protected BaseController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            if (User.Identity?.IsAuthenticated == true)
            {
                // Kullanıcı ID
                var kullaniciIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!string.IsNullOrEmpty(kullaniciIdClaim))
                {
                    CurrentKullaniciId = int.Parse(kullaniciIdClaim);
                }

                // Şirket ID - Session'dan veya Claim'den
                if (HttpContext.Session.GetInt32("CurrentSirketId").HasValue)
                {
                    CurrentSirketId = HttpContext.Session.GetInt32("CurrentSirketId").Value;
                }
                else
                {
                    var sirketIdClaim = User.FindFirst("SirketId")?.Value;
                    if (!string.IsNullOrEmpty(sirketIdClaim))
                    {
                        CurrentSirketId = int.Parse(sirketIdClaim);
                        HttpContext.Session.SetInt32("CurrentSirketId", CurrentSirketId);
                    }
                }

                // ViewBag'e şirket bilgilerini ekle
                ViewBag.CurrentSirketId = CurrentSirketId;
                ViewBag.IsAdmin = User.IsInRole("Admin");
            }
        }

        protected async Task LogAction(string islemTipi, string modul, string aciklama)
        {
            try
            {
                var log = new PDKS.Data.Entities.Log
                {
                    KullaniciId = CurrentKullaniciId,
                    Islem = islemTipi,        
                    Modul = modul,
                    Detay = aciklama,         
                    Tarih = DateTime.UtcNow,  
                    IpAdres = HttpContext.Connection.RemoteIpAddress?.ToString() 
                };

                await _unitOfWork.Loglar.AddAsync(log); 
                await _unitOfWork.SaveChangesAsync();
            }
            catch
            {
                // Log hatası uygulamayı etkilememeli
            }
        }
    }
}