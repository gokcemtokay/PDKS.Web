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
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
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
                g.GirisZamani.HasValue &&
                g.GirisZamani.Value.Date == today);

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
                    GunSayisi = i.GunSayisi,
                    OnayDurumu = i.OnayDurumu,
                    OlusturmaTarihi = i.OlusturmaTarihi
                })
                .ToList();

            // Kullanıcı bildirimlerini getir
            var kullaniciId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var okunmamisBildirimler = await _unitOfWork.Bildirimler.CountAsync(b =>
                b.KullaniciId == kullaniciId && !b.Okundu);

            ViewBag.OkunmamisBildirimler = okunmamisBildirimler;

            return View(dashboard);
        }

        [HttpGet]
        public async Task<IActionResult> Bildirimler()
        {
            var kullaniciId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
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

            return View(bildirimList);
        }

        [HttpPost]
        public async Task<IActionResult> BildirimOkunduIsaretle(int id)
        {
            var bildirim = await _unitOfWork.Bildirimler.GetByIdAsync(id);
            if (bildirim != null)
            {
                bildirim.Okundu = true;
                _unitOfWork.Bildirimler.Update(bildirim);
                await _unitOfWork.SaveChangesAsync();
            }

            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> TumBildirimleriOkunduIsaretle()
        {
            var kullaniciId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var bildirimler = await _unitOfWork.Bildirimler.FindAsync(b =>
                b.KullaniciId == kullaniciId && !b.Okundu);

            foreach (var bildirim in bildirimler)
            {
                bildirim.Okundu = true;
                _unitOfWork.Bildirimler.Update(bildirim);
            }

            await _unitOfWork.SaveChangesAsync();
            return Json(new { success = true });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}