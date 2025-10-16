using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PDKS.Business.Services;
using PDKS.Data.Entities;
using PDKS.Data.Repositories;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PDKS.WebUI.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;

        public AuthController(IAuthService authService, IUnitOfWork unitOfWork)
        {
            _authService = authService;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password, string returnUrl = null)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                TempData["Error"] = "Email ve şifre alanları zorunludur";
                return View();
            }

            var (success, user, message) = await _authService.LoginAsync(email, password);

            if (!success)
            {
                TempData["Error"] = message;
                return View();
            }

            // Load user details with relations
            var kullanici = await _unitOfWork.Kullanicilar.GetByIdAsync(user.Id);
            var personel = kullanici?.PersonelId.HasValue == true
                ? await _unitOfWork.Personeller.GetByIdAsync(kullanici.PersonelId.Value)
                : null;
            var rol = await _unitOfWork.Roller.GetByIdAsync(kullanici.RolId);

            // Create claims
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, kullanici.Id.ToString()),
        new Claim(ClaimTypes.Name, personel?.AdSoyad ?? kullanici.KullaniciAdi),
        new Claim(ClaimTypes.Email, kullanici.Email),
        new Claim(ClaimTypes.Role, rol.RolAdi),
        new Claim("PersonelId", personel?.Id.ToString() ?? "0"),
        new Claim("Departman", personel?.Departman?.Ad ?? "")
    };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            // Log the login
            await _unitOfWork.Loglar.AddAsync(new Log
            {
                KullaniciId = kullanici.Id,
                Islem = "Giriş",
                Modul = "Auth",
                Detay = $"{kullanici.KullaniciAdi} sisteme giriş yaptı",
                IpAdres = HttpContext.Connection.RemoteIpAddress?.ToString(),
                Tarih = DateTime.UtcNow
            });
            await _unitOfWork.SaveChangesAsync();

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var kullaniciId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            if (kullaniciId > 0)
            {
                await _unitOfWork.Loglar.AddAsync(new Data.Entities.Log
                {
                    KullaniciId = kullaniciId,
                    Islem = "Çıkış",
                    Modul = "Auth",
                    Detay = "Kullanıcı sistemden çıkış yaptı",
                    IpAdres = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Tarih = DateTime.UtcNow
                });
                await _unitOfWork.SaveChangesAsync();
            }

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [HttpGet]
        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword(string oldPassword, string newPassword, string confirmPassword)
        {
            if (string.IsNullOrWhiteSpace(oldPassword) || string.IsNullOrWhiteSpace(newPassword))
            {
                TempData["Error"] = "Tüm alanları doldurunuz";
                return View();
            }

            if (newPassword != confirmPassword)
            {
                TempData["Error"] = "Yeni şifreler eşleşmiyor";
                return View();
            }

            if (newPassword.Length < 6)
            {
                TempData["Error"] = "Şifre en az 6 karakter olmalıdır";
                return View();
            }

            var kullaniciId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var success = await _authService.ChangePasswordAsync(kullaniciId, oldPassword, newPassword);

            if (!success)
            {
                TempData["Error"] = "Mevcut şifre hatalı";
                return View();
            }

            TempData["Success"] = "Şifre başarıyla değiştirildi";
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}