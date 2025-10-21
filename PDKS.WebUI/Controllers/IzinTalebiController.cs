using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PDKS.Data.Context;
using PDKS.Data.Entities;

namespace PDKS.WebUI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class IzinTalebiController : ControllerBase
    {
        private readonly PDKSDbContext _context;

        public IzinTalebiController(PDKSDbContext context)
        {
            _context = context;
        }

        // GET: api/IzinTalebi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetIzinler()
        {
            var izinler = await _context.Izinler
                .Include(i => i.Personel)
                .Select(i => new
                {
                    i.Id,
                    PersonelAdi = i.Personel.AdSoyad, // ✅ Düzeltildi
                    i.PersonelId,
                    i.IzinTipi,
                    i.BaslangicTarihi,
                    i.BitisTarihi,
                    GunSayisi = CalculateGunSayisi(i.BaslangicTarihi, i.BitisTarihi), // ✅ Hesaplama
                    i.OnayDurumu,
                    i.Aciklama
                })
                .OrderByDescending(i => i.BaslangicTarihi)
                .ToListAsync();

            return Ok(izinler);
        }

        // GET: api/IzinTalebi/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<object>> GetIzin(int id)
        //{
        //    var izin = await _context.Izinler
        //        .Include(i => i.Personel)
        //        .Where(i => i.Id == id)
        //        .Select(i => new
        //        {
        //            i.Id,
        //            PersonelAdi = i.Personel.AdSoyad, // ✅ Düzeltildi
        //            i.PersonelId,
        //            i.IzinTipi,
        //            i.BaslangicTarihi,
        //            i.BitisTarihi,
        //            GunSayisi = CalculateGunSayisi(i.BaslangicTarihi, i.BitisTarihi), // ✅ Hesaplama
        //            i.OnayDurumu,
        //            i.Aciklama,
        //            // VekilPersonelId = null, // ✅ Entity'de yoksa kaldır
        //            OnayAkisi = _context.OnayAkislari
        //                .Where(o => o.OnayTipi == "Izin" && o.ReferansId == i.Id)
        //                .Select(o => new
        //                {
        //                    o.Sira,
        //                    OnaylayiciAdi = o.Onaylayici.AdSoyad, // ✅ Düzeltildi
        //                    o.OnayDurumu,
        //                    o.OnayTarihi
        //                })
        //                .ToList()
        //        })
        //        .FirstOrDefaultAsync();

        //    if (izin == null)
        //        return NotFound();

        //    return Ok(izin);
        //}

        // POST: api/IzinTalebi
        //[HttpPost]
        //public async Task<ActionResult<Izin>> PostIzin([FromBody] IzinTalebiDTO dto)
        //{
        //    // İzin bakiyesi kontrolü
        //    var izinHakki = await _context.IzinHaklari
        //        .FirstOrDefaultAsync(ih => ih.PersonelId == dto.PersonelId && ih.Yil == DateTime.UtcNow.Year);

        //    if (izinHakki == null)
        //    {
        //        return BadRequest("İzin hakkı bulunamadı. Lütfen İK ile iletişime geçin.");
        //    }

        //    // Gün sayısını hesapla
        //    decimal gunSayisi = CalculateGunSayisi(dto.BaslangicTarihi, dto.BitisTarihi);

        //    if (izinHakki.KalanIzin < gunSayisi)
        //    {
        //        return BadRequest($"Yetersiz izin bakiyesi. Kalan izin: {izinHakki.KalanIzin} gün");
        //    }

        //    // İzin talebi oluştur
        //    var izin = new Izin
        //    {
        //        PersonelId = dto.PersonelId,
        //        IzinTipi = dto.IzinTipi,
        //        BaslangicTarihi = dto.BaslangicTarihi,
        //        BitisTarihi = dto.BitisTarihi,
        //        Aciklama = dto.Aciklama,
        //        // VekilPersonelId = dto.VekilPersonelId, // ✅ Entity'de yoksa kaldır
        //        OnayDurumu = "Beklemede"
        //    };

        //    _context.Izinler.Add(izin);
        //    await _context.SaveChangesAsync();

            // Onay akışı oluştur
            //if (dto.OnaylayiciIds != null && dto.OnaylayiciIds.Any())
            //{
            //    for (int i = 0; i < dto.OnaylayiciIds.Count; i++)
            //    {
            //        var onayAkisi = new OnayAkisi
            //        {
            //            OnayTipi = "Izin",
            //            ReferansId = izin.Id,
            //            Sira = i + 1,
            //            OnaylayiciPersonelId = dto.OnaylayiciIds[i],
            //            SirketId = dto.SirketId
            //        };
            //        _context.OnayAkislari.Add(onayAkisi);
            //    }
            //    await _context.SaveChangesAsync();

            //    // İlk onaylayıcıya bildirim gönder
            //    await SendOnayBildirimi(dto.OnaylayiciIds[0], "Izin", izin.Id);
            //}

        //    return CreatedAtAction(nameof(GetIzin), new { id = izin.Id }, izin);
        //}

        // GET: api/IzinTalebi/Personel/{personelId}
        [HttpGet("Personel/{personelId}")]
        public async Task<ActionResult<object>> GetPersonelIzinler(int personelId)
        {
            var izinler = await _context.Izinler
                .Where(i => i.PersonelId == personelId)
                .OrderByDescending(i => i.BaslangicTarihi)
                .Select(i => new
                {
                    i.Id,
                    i.IzinTipi,
                    i.BaslangicTarihi,
                    i.BitisTarihi,
                    GunSayisi = CalculateGunSayisi(i.BaslangicTarihi, i.BitisTarihi), // ✅ Hesaplama
                    i.OnayDurumu,
                    i.Aciklama
                })
                .ToListAsync();

            return Ok(izinler);
        }

        // GET: api/IzinTalebi/Bakiye/{personelId}
        [HttpGet("Bakiye/{personelId}")]
        public async Task<ActionResult<object>> GetIzinBakiyesi(int personelId)
        {
            var izinHakki = await _context.IzinHaklari
                .Where(ih => ih.PersonelId == personelId && ih.Yil == DateTime.UtcNow.Year)
                .Select(ih => new
                {
                    ih.Yil,
                    ih.YillikIzinGun,
                    ih.KullanilanIzin,
                    ih.KalanIzin,
                    ih.MazeretiIzin,
                    ih.HastalikIzin,
                    ih.UcretsizIzin
                })
                .FirstOrDefaultAsync();

            if (izinHakki == null)
                return NotFound("İzin hakkı bulunamadı");

            return Ok(izinHakki);
        }

        // Helper Methods
        private decimal CalculateGunSayisi(DateTime baslangic, DateTime bitis)
        {
            return (decimal)(bitis - baslangic).TotalDays + 1;
        }

        private async Task SendOnayBildirimi(int onaylayiciPersonelId, string talepTipi, int referansId)
        {
            var kullanici = await _context.Kullanicilar
                .FirstOrDefaultAsync(k => k.PersonelId == onaylayiciPersonelId);

            if (kullanici != null)
            {
                var bildirim = new Bildirim
                {
                    KullaniciId = kullanici.Id,
                    Baslik = $"Yeni {talepTipi} Talebi",
                    Mesaj = $"Onayınız bekleyen yeni bir {talepTipi} talebi var.",
                    Tip = "Bilgi",
                    ReferansTip = talepTipi,
                    ReferansId = referansId
                };

                _context.Bildirimler.Add(bildirim);
                await _context.SaveChangesAsync();
            }
        }
    }

    // DTO
    public class IzinTalebiDTO
    {
        public int PersonelId { get; set; }
        public string IzinTipi { get; set; }
        public DateTime BaslangicTarihi { get; set; }
        public DateTime BitisTarihi { get; set; }
        public string? Aciklama { get; set; }
        // public int? VekilPersonelId { get; set; } // ✅ Entity'de yoksa kaldır
        public List<int> OnaylayiciIds { get; set; }
        public int SirketId { get; set; }
    }
}