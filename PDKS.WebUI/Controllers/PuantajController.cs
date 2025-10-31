using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PDKS.Business.DTOs;
using PDKS.Business.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PDKS.WebUI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PuantajController : ControllerBase
    {
        private readonly IPuantajService _puantajService;

        public PuantajController(IPuantajService puantajService)
        {
            _puantajService = puantajService;
        }

        private int GetCurrentSirketId()
        {
            var sirketIdClaim = User.Claims.FirstOrDefault(c => c.Type == "sirketId");
            if (sirketIdClaim != null && int.TryParse(sirketIdClaim.Value, out int sirketId))
            {
                return sirketId;
            }
            throw new UnauthorizedAccessException("Şirket ID bulunamadı");
        }

        /// <summary>
        /// Belirli dönem için puantaj listesini getirir
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> GetAll([FromQuery] int yil, [FromQuery] int ay)
        {
            try
            {
                var sirketId = GetCurrentSirketId();
                var puantajlar = await _puantajService.GetAllAsync(sirketId, yil, ay);
                return Ok(puantajlar);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Hata: " + ex.Message });
            }
        }

        /// <summary>
        /// Puantaj detayını getirir
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            try
            {
                var puantaj = await _puantajService.GetByIdAsync(id);
                if (puantaj == null)
                    return NotFound(new { message = "Puantaj bulunamadı" });

                var sirketId = GetCurrentSirketId();
                if (puantaj.SirketId != sirketId)
                    return Forbid();

                return Ok(puantaj);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Hata: " + ex.Message });
            }
        }

        /// <summary>
        /// Personel için puantaj detayını getirir
        /// </summary>
        [HttpGet("personel/{personelId}")]
        public async Task<ActionResult> GetByPersonel(int personelId, [FromQuery] int yil, [FromQuery] int ay)
        {
            try
            {
                var puantaj = await _puantajService.GetByPersonelAsync(personelId, yil, ay);
                if (puantaj == null)
                    return NotFound(new { message = "Puantaj bulunamadı" });

                var sirketId = GetCurrentSirketId();
                if (puantaj.SirketId != sirketId)
                    return Forbid();

                return Ok(puantaj);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Hata: " + ex.Message });
            }
        }

        /// <summary>
        /// Tek personel için puantaj oluşturur
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] PuantajCreateDTO dto)
        {
            try
            {
                var sirketId = GetCurrentSirketId();
                dto.SirketId = sirketId;

                var puantajId = await _puantajService.OlusturAsync(dto);
                var puantaj = await _puantajService.GetByIdAsync(puantajId);

                return CreatedAtAction(nameof(GetById), new { id = puantajId }, puantaj);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Toplu puantaj oluşturur
        /// </summary>
        [HttpPost("toplu")]
        public async Task<ActionResult> CreateBulk([FromBody] PuantajTopluOlusturDTO dto)
        {
            try
            {
                var sirketId = GetCurrentSirketId();
                dto.SirketId = sirketId;

                var olusturulanIdler = await _puantajService.TopluOlusturAsync(dto);
                return Ok(new 
                { 
                    message = $"{olusturulanIdler.Count} adet puantaj oluşturuldu",
                    ids = olusturulanIdler 
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Puantajı yeniden hesaplar
        /// </summary>
        [HttpPost("{id}/yeniden-hesapla")]
        public async Task<ActionResult> Recalculate(int id)
        {
            try
            {
                var puantaj = await _puantajService.GetByIdAsync(id);
                if (puantaj == null)
                    return NotFound(new { message = "Puantaj bulunamadı" });

                var sirketId = GetCurrentSirketId();
                if (puantaj.SirketId != sirketId)
                    return Forbid();

                await _puantajService.YenidenHesaplaAsync(id);
                var guncelPuantaj = await _puantajService.GetByIdAsync(id);

                return Ok(new 
                { 
                    message = "Puantaj yeniden hesaplandı",
                    puantaj = guncelPuantaj 
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Puantajı onaylar/onayı kaldırır
        /// </summary>
        [HttpPost("{id}/onayla")]
        public async Task<ActionResult> Approve(int id, [FromBody] PuantajOnayDTO dto)
        {
            try
            {
                var puantaj = await _puantajService.GetByIdAsync(id);
                if (puantaj == null)
                    return NotFound(new { message = "Puantaj bulunamadı" });

                var sirketId = GetCurrentSirketId();
                if (puantaj.SirketId != sirketId)
                    return Forbid();

                dto.PuantajId = id;
                await _puantajService.OnaylaAsync(dto);

                return Ok(new { message = dto.Onayla ? "Puantaj onaylandı" : "Puantaj onayı kaldırıldı" });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Puantaj istatistiklerini getirir
        /// </summary>
        [HttpGet("istatistik")]
        public async Task<ActionResult> GetStatistics([FromQuery] int yil, [FromQuery] int ay)
        {
            try
            {
                var sirketId = GetCurrentSirketId();
                var istatistik = await _puantajService.GetIstatistikAsync(sirketId, yil, ay);
                return Ok(istatistik);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Hata: " + ex.Message });
            }
        }

        /// <summary>
        /// Geç kalanlar raporunu getirir
        /// </summary>
        [HttpGet("rapor/gec-kalanlar")]
        public async Task<ActionResult> GetLateArrivals([FromQuery] DateTime baslangic, [FromQuery] DateTime bitis)
        {
            try
            {
                var sirketId = GetCurrentSirketId();
                var rapor = await _puantajService.GetGecKalanlarRaporuAsync(sirketId, baslangic, bitis);
                return Ok(rapor);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Hata: " + ex.Message });
            }
        }

        /// <summary>
        /// Erken çıkanlar raporunu getirir
        /// </summary>
        [HttpGet("rapor/erken-cikanlar")]
        public async Task<ActionResult> GetEarlyLeavers([FromQuery] DateTime baslangic, [FromQuery] DateTime bitis)
        {
            try
            {
                var sirketId = GetCurrentSirketId();
                var rapor = await _puantajService.GetErkenCikanlarRaporuAsync(sirketId, baslangic, bitis);
                return Ok(rapor);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Hata: " + ex.Message });
            }
        }

        /// <summary>
        /// Fazla mesai raporunu getirir
        /// </summary>
        [HttpGet("rapor/fazla-mesai")]
        public async Task<ActionResult> GetOvertime([FromQuery] DateTime baslangic, [FromQuery] DateTime bitis)
        {
            try
            {
                var sirketId = GetCurrentSirketId();
                var rapor = await _puantajService.GetFazlaMesaiRaporuAsync(sirketId, baslangic, bitis);
                return Ok(rapor);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Hata: " + ex.Message });
            }
        }

        /// <summary>
        /// Devamsızlık raporunu getirir
        /// </summary>
        [HttpGet("rapor/devamsizlik")]
        public async Task<ActionResult> GetAbsences([FromQuery] DateTime baslangic, [FromQuery] DateTime bitis)
        {
            try
            {
                var sirketId = GetCurrentSirketId();
                var rapor = await _puantajService.GetDevamsizlikRaporuAsync(sirketId, baslangic, bitis);
                return Ok(rapor);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Hata: " + ex.Message });
            }
        }
    }
}
