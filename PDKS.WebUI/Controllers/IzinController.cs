using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PDKS.Business.DTOs;
using PDKS.Data.Repositories;

namespace PDKS.WebUI.Controllers
{
    [Authorize]
    public class IzinController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public IzinController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var izinler = await _unitOfWork.Izinler.GetAllAsync();

            // Filter based on role
            if (!User.IsInRole("Admin") && !User.IsInRole("IK"))
            {
                var personelId = int.Parse(User.FindFirst("PersonelId")?.Value ?? "0");

                if (User.IsInRole("Yönetici"))
                {
                    // Manager sees their department's leaves
                    var departman = User.FindFirst("Departman")?.Value;
                    izinler = izinler.Where(i => i.Personel.Departman.Ad == departman).ToList();
                }
                else
                {
                    // Employee sees only their own leaves
                    izinler = izinler.Where(i => i.PersonelId == personelId).ToList();
                }
            }

            var izinListDto = izinler.Select(i => new IzinListDTO
            {
                Id = i.Id,
                PersonelId = i.PersonelId,
                PersonelAdi = i.Personel.AdSoyad,
                IzinTipi = i.IzinTipi,
                BaslangicTarihi = i.BaslangicTarihi,
                BitisTarihi = i.BitisTarihi,
                GunSayisi = i.GunSayisi,
                Aciklama = i.Aciklama,
                OnayDurumu = i.OnayDurumu,
                OnaylayanAdi = i.OnaylayanKullanici?.Personel?.AdSoyad,
                OnayTarihi = i.OnayTarihi,
                OlusturmaTarihi = i.OlusturmaTarihi
            }).OrderByDescending(i => i.OlusturmaTarihi);

            return View(izinListDto);
        }

        //private async Task LoadViewBagData()
        //{
        //    // Burada ViewBag'e veri doldurabilirsin.
        //    // Örneğin:
        //    ViewBag.Departmanlar = await _context.Departmanlar
        //        .OrderBy(d => d.Adi)
        //        .ToListAsync();

        //    ViewBag.Pozisyonlar = await _context.Pozisyonlar
        //        .OrderBy(p => p.Adi)
        //        .ToListAsync();
        //}

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            //await LoadViewBagData();

            var dto = new IzinCreateDTO
            {
                BaslangicTarihi = DateTime.Today,
                BitisTarihi = DateTime.Today.AddDays(1)
            };

            // If not admin/ik, set current user as personel
            if (!User.IsInRole("Admin") && !User.IsInRole("IK"))
            {
                dto.PersonelId = int.Parse(User.FindFirst("PersonelId")?.Value ?? "0");
            }

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IzinCreateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                //await LoadViewBagData();
                return View(dto);
            }

            try
            {
                // Calculate days
                var gunSayisi = (dto.BitisTarihi - dto.BaslangicTarihi).Days + 1;

                var izin = new Data.Entities.Izin
                {
                    PersonelId = dto.PersonelId,
                    IzinTipi = dto.IzinTipi,
                    BaslangicTarihi = dto.BaslangicTarihi,
                    BitisTarihi = dto.BitisTarihi,
                    GunSayisi = gunSayisi,
                    Aciklama = dto.Aciklama,
                    OnayDurumu = "Beklemede",
                    OlusturmaTarihi = DateTime.UtcNow
                };

                await _unitOfWork.Izinler.AddAsync(izin);
                await _unitOfWork.SaveChangesAsync();

                // Create notification for managers
                var personel = await _unitOfWork.Personeller.GetByIdAsync(dto.PersonelId);
                var yoneticiler = await _unitOfWork.Kullanicilar.FindAsync(k =>
                    (k.Rol.RolAdi == "Admin" || k.Rol.RolAdi == "IK" || k.Rol.RolAdi == "Yönetici")
                    && k.Aktif);

                foreach (var yonetici in yoneticiler)
                {
                    await _unitOfWork.Bildirimler.AddAsync(new Data.Entities.Bildirim
                    {
                        KullaniciId = yonetici.Id,
                        Baslik = "Yeni İzin Talebi",
                        Mesaj = $"{personel.AdSoyad} ({gunSayisi} gün) izin talebi oluşturdu.",
                        Tip = "Info",
                        Okundu = false,
                        OlusturmaTarihi = DateTime.UtcNow
                    });
                }

                await _unitOfWork.SaveChangesAsync();

                //// Log the action
                //await LogAction("İzin Talebi Oluşturma", "Izin",
                //    $"{personel.AdSoyad} için izin talebi oluşturuldu");

                TempData["Success"] = "İzin talebi başarıyla oluşturuldu";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                //await LoadViewBagData();
                return View(dto);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var izin = await _unitOfWork.Izinler.GetByIdAsync(id);
            if (izin == null)
                return NotFound();

            // Check permission
            if (!User.IsInRole("Admin") && !User.IsInRole("IK") && !User.IsInRole("Yönetici"))
            {
                var personelId = int.Parse(User.FindFirst("PersonelId")?.Value ?? "0");
                if (izin.PersonelId != personelId)
                    return Forbid();
            }

            var dto = new IzinListDTO
            {
                Id = izin.Id,
                PersonelId = izin.PersonelId,
                PersonelAdi = izin.Personel.AdSoyad,
                IzinTipi = izin.IzinTipi,
                BaslangicTarihi = izin.BaslangicTarihi,
                BitisTarihi = izin.BitisTarihi,
                GunSayisi = izin.GunSayisi,
                Aciklama = izin.Aciklama,
                OnayDurumu = izin.OnayDurumu,

                // Replace the problematic line in the Details method with the following:
                OnaylayanAdi = izin.OnaylayanKullanici?.Personel?.AdSoyad,
                OnayTarihi = izin.OnayTarihi,
                OlusturmaTarihi = izin.OlusturmaTarihi
            };

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,IK,Yönetici")]
        public async Task<IActionResult> Onayla(int id)
        {
            try
            {
                var izin = await _unitOfWork.Izinler.GetByIdAsync(id);
                if (izin == null)
                    return NotFound();

                var kullaniciId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

                izin.OnayDurumu = "Onaylandı";
                izin.OnaylayanId = kullaniciId;
                izin.OnayTarihi = DateTime.UtcNow;

                _unitOfWork.Izinler.Update(izin);
                await _unitOfWork.SaveChangesAsync();

                // Bildirim ekle
                var personelKullanici = await _unitOfWork.Kullanicilar.FirstOrDefaultAsync(k =>
                    k.PersonelId == izin.PersonelId);

                if (personelKullanici != null)
                {
                    await _unitOfWork.Bildirimler.AddAsync(new Data.Entities.Bildirim
                    {
                        KullaniciId = personelKullanici.Id,
                        Baslik = "İzin Talebi Onaylandı",
                        Mesaj = $"{izin.GunSayisi} gün izin talebiniz onaylandı.",
                        Tip = "Success",
                        Okundu = false,
                        OlusturmaTarihi = DateTime.UtcNow
                    });
                    await _unitOfWork.SaveChangesAsync();
                }

                TempData["Success"] = "İzin başarıyla onaylandı";
            }
            catch (Exception ex)
            {
                // Loglama yapılmalı (burada sadece TempData)
                TempData["Error"] = "İzin onaylanırken bir hata oluştu: " + ex.Message;
            }

            // her durumda bir IActionResult dön
            return RedirectToAction("Index"); // veya ilgili liste/ayrıntı sayfası
        }
    }
}