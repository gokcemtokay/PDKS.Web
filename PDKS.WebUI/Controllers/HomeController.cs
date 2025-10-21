using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PDKS.Business.DTOs;
using PDKS.Data.Repositories;

namespace PDKS.WebUI.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class HomeController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboardData()
        {
            try
            {
                var dashboard = new DashboardDTO();
                var today = DateTime.Today;

                // Aktif personel sayısı
                dashboard.AktifPersonelSayisi = await _unitOfWork.Personeller.CountAsync(p => p.Durum);

                // Bugün giriş yapmayanlar
                var aktifPersoneller = await _unitOfWork.Personeller.FindAsync(p => p.Durum);
                var bugunGirisYapanlar = await _unitOfWork.GirisCikislar.FindAsync(g =>
                    g.GirisZamani.HasValue && g.GirisZamani.Value.Date == today);

                dashboard.BugunGelmeyenler = aktifPersoneller.Count() - bugunGirisYapanlar.Count();

                // Geç gelenler
                dashboard.GecGelenler = await _unitOfWork.GirisCikislar.CountAsync(g =>
                    g.GirisZamani.HasValue &&
                    g.GirisZamani.Value.Date == today &&
                    g.GecKalmaSuresi > 0);

                // Erken çıkanlar
                dashboard.ErkenCikanlar = await _unitOfWork.GirisCikislar.CountAsync(g =>
                    g.GirisZamani.HasValue &&
                    g.GirisZamani.Value.Date == today &&
                    g.ErkenCikisSuresi > 0);

                // Mesaiye kalanlar
                dashboard.MesaiyeKalanlar = await _unitOfWork.GirisCikislar.CountAsync(g =>
                    g.GirisZamani.HasValue &&
                    g.GirisZamani.Value.Date == today &&
                    g.FazlaMesaiSuresi > 0);

                // İzinli personel
                dashboard.IzinliPersonel = await _unitOfWork.Izinler.CountAsync(i =>
                    i.BaslangicTarihi <= today &&
                    i.BitisTarihi >= today &&
                    i.OnayDurumu == "Onaylandı");

                // Bekleyen izin talepleri
                dashboard.BekleyenIzinTalepleri = await _unitOfWork.Izinler.CountAsync(i =>
                    i.OnayDurumu == "Beklemede");

                // Aktif cihaz sayısı
                dashboard.AktifCihazSayisi = await _unitOfWork.Cihazlar.CountAsync(c => c.Durum);

                // Son giriş-çıkışlar (Son 10)
                var sonGirisCikislar = await _unitOfWork.GirisCikislar.FindAsync(g =>
                    g.GirisZamani.HasValue); // Tarih filtresi kaldırıldı, en son hareketleri görmek daha mantıklı olabilir.

                dashboard.SonGirisCikislar = sonGirisCikislar
                    .OrderByDescending(g => g.GirisZamani)
                    .Take(10)
                    .Select(g => new GirisCikisListDTO
                    {
                        Id = g.Id,
                        PersonelId = g.PersonelId,
                        PersonelAdi = g.Personel?.AdSoyad,
                        SicilNo = g.Personel?.SicilNo,
                        GirisZamani = g.GirisZamani,
                        CikisZamani = g.CikisZamani,
                        Durum = g.Durum,
                        GecKalmaSuresi = g.GecKalmaSuresi,
                        FazlaMesaiSuresi = g.FazlaMesaiSuresi
                    })
                    .ToList();

                // Bekleyen izinler (Son 5)
                var bekleyenIzinler = await _unitOfWork.Izinler.FindAsync(i =>
                    i.OnayDurumu == "Beklemede");

                dashboard.BekleyenIzinler = bekleyenIzinler
                    .OrderByDescending(i => i.OlusturmaTarihi)
                    .Take(5)
                    .Select(i => new IzinListDTO
                    {
                        Id = i.Id,
                        PersonelId = i.PersonelId,
                        PersonelAdi = i.Personel?.AdSoyad,
                        IzinTipi = i.IzinTipi,
                        BaslangicTarihi = i.BaslangicTarihi,
                        BitisTarihi = i.BitisTarihi,
                        GunSayisi = i.IzinGunSayisi,
                        OnayDurumu = i.OnayDurumu,
                        OlusturmaTarihi = i.OlusturmaTarihi
                    })
                    .ToList();

                return Ok(dashboard);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("bildirimler")]
        public async Task<IActionResult> GetBildirimler()
        {
            var kullaniciId = GetCurrentUserId();
            var bildirimler = await _unitOfWork.Bildirimler.FindAsync(b => b.KullaniciId == kullaniciId);

            var bildirimList = bildirimler
                .OrderByDescending(b => b.OlusturmaTarihi)
                .Select(b => new
                {
                    b.Id,
                    b.Baslik,
                    b.Mesaj,
                    b.Tip,
                    b.Okundu,
                    b.OlusturmaTarihi
                })
                .ToList();

            return Ok(bildirimList);
        }

        [HttpPost("bildirim-okundu/{id}")]
        public async Task<IActionResult> BildirimOkunduIsaretle(int id)
        {
            var bildirim = await _unitOfWork.Bildirimler.GetByIdAsync(id);
            if (bildirim == null)
            {
                return NotFound();
            }

            // Güvenlik kontrolü: Bildirim sadece o kullanıcıya aitse işlem yapılmalı.
            if (bildirim.KullaniciId != GetCurrentUserId())
            {
                return Forbid(); // 403 Forbidden
            }

            bildirim.Okundu = true;
            _unitOfWork.Bildirimler.Update(bildirim);
            await _unitOfWork.SaveChangesAsync();

            return Ok(new { success = true });
        }

        [HttpPost("tum-bildirimleri-okundu")]
        public async Task<IActionResult> TumBildirimleriOkunduIsaretle()
        {
            var kullaniciId = GetCurrentUserId();
            var bildirimler = await _unitOfWork.Bildirimler.FindAsync(b =>
                b.KullaniciId == kullaniciId && !b.Okundu);

            foreach (var bildirim in bildirimler)
            {
                bildirim.Okundu = true;
                _unitOfWork.Bildirimler.Update(bildirim);
            }

            await _unitOfWork.SaveChangesAsync();
            return Ok(new { success = true });
        }

        private int GetCurrentUserId()
        {
            // Bu metodu önceki adımlarda SirketController'a eklemiştik,
            // daha merkezi bir yere taşımak (örn: bir helper sınıfı) daha iyi bir pratik olacaktır.
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier || c.Type == "sub");
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }
            throw new InvalidOperationException("User ID could not be found in token.");
        }
    }
}