using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PDKS.Business.DTOs;
using PDKS.Business.Services;

namespace PDKS.WebUI.Controllers
{
    [Authorize(Roles = "Admin,IK")]
    [ApiController]
#if DEBUG
    [Route("api/[controller]")]
#else
    [Route("[controller]")]
#endif
    [Produces("application/json")]
    [Consumes("application/json")]
    public class PuantajController : ControllerBase
    {
        private readonly IPuantajService _puantajService;

        public PuantajController(IPuantajService puantajService)
        {
            _puantajService = puantajService;
        }

        // GET: api/Puantaj/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<PuantajDetailDTO>> GetById(int id)
        {
            try
            {
                var puantaj = await _puantajService.GetByIdAsync(id);
                if (puantaj == null)
                    return NotFound($"ID {id} olan puantaj bulunamadı.");

                return Ok(puantaj);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Hata: {ex.Message}");
            }
        }

        // GET: api/Puantaj/personel/{personelId}
        [HttpGet("personel/{personelId}")]
        public async Task<ActionResult<IEnumerable<PuantajListDTO>>> GetByPersonel(int personelId)
        {
            try
            {
                var puantajlar = await _puantajService.GetByPersonelAsync(personelId);
                return Ok(puantajlar);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Hata: {ex.Message}");
            }
        }

        // GET: api/Puantaj/donem?yil=2025&ay=1&departmanId=5
        [HttpGet("donem")]
        public async Task<ActionResult<IEnumerable<PuantajListDTO>>> GetByDonem(
            [FromQuery] int yil, 
            [FromQuery] int ay, 
            [FromQuery] int? departmanId = null)
        {
            try
            {
                var puantajlar = await _puantajService.GetByDonemAsync(yil, ay, departmanId);
                return Ok(puantajlar);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Hata: {ex.Message}");
            }
        }

        // GET: api/Puantaj/personel/{personelId}/donem?yil=2025&ay=1
        [HttpGet("personel/{personelId}/donem")]
        public async Task<ActionResult<PuantajDetailDTO>> GetByPersonelVeDonem(
            int personelId, 
            [FromQuery] int yil, 
            [FromQuery] int ay)
        {
            try
            {
                var puantaj = await _puantajService.GetByPersonelVeDonemAsync(personelId, yil, ay);
                if (puantaj == null)
                    return NotFound("Belirtilen dönem için puantaj bulunamadı.");

                return Ok(puantaj);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Hata: {ex.Message}");
            }
        }

        // POST: api/Puantaj/hesapla
        [HttpPost("hesapla")]
        public async Task<ActionResult<int>> HesaplaPuantaj([FromBody] PuantajHesaplaDTO dto)
        {
            try
            {
                var puantajId = await _puantajService.HesaplaPuantajAsync(dto);
                var puantaj = await _puantajService.GetByIdAsync(puantajId);
                return CreatedAtAction(nameof(GetById), new { id = puantajId }, puantaj);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST: api/Puantaj/toplu-hesapla
        [HttpPost("toplu-hesapla")]
        public async Task<ActionResult<List<int>>> TopluPuantajHesapla([FromBody] TopluPuantajHesaplaDTO dto)
        {
            try
            {
                var puantajIdler = await _puantajService.TopluPuantajHesaplaAsync(dto);
                return Ok(new 
                { 
                    message = $"{puantajIdler.Count} adet puantaj hesaplandı.",
                    puantajIdler = puantajIdler
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Hata: {ex.Message}");
            }
        }

        // PUT: api/Puantaj/{id}/yeniden-hesapla
        [HttpPut("{id}/yeniden-hesapla")]
        public async Task<ActionResult<PuantajDetailDTO>> YenidenHesapla(int id)
        {
            try
            {
                var puantaj = await _puantajService.YenidenHesaplaAsync(id);
                return Ok(puantaj);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/Puantaj/onayla
        [HttpPut("onayla")]
        public async Task<ActionResult> OnaylaPuantaj([FromBody] PuantajOnayDTO dto)
        {
            try
            {
                var sonuc = await _puantajService.OnaylaAsync(dto);
                if (!sonuc)
                    return NotFound("Puantaj bulunamadı.");

                return Ok(new { message = "Puantaj başarıyla onaylandı." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/Puantaj/{id}/onay-iptal
        [HttpPut("{id}/onay-iptal")]
        public async Task<ActionResult> OnayIptal(int id)
        {
            try
            {
                var sonuc = await _puantajService.OnayIptalAsync(id);
                if (!sonuc)
                    return NotFound("Puantaj bulunamadı.");

                return Ok(new { message = "Puantaj onayı iptal edildi." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/Puantaj/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var sonuc = await _puantajService.DeleteAsync(id);
                if (!sonuc)
                    return NotFound("Puantaj bulunamadı.");

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Hata: {ex.Message}");
            }
        }

        // GET: api/Puantaj/{id}/detaylar
        [HttpGet("{id}/detaylar")]
        public async Task<ActionResult<List<PuantajDetayDTO>>> GetGunlukDetaylar(int id)
        {
            try
            {
                var detaylar = await _puantajService.GetGunlukDetaylarAsync(id);
                return Ok(detaylar);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Hata: {ex.Message}");
            }
        }

        // GET: api/Puantaj/personel/{personelId}/detay?tarih=2025-01-15
        [HttpGet("personel/{personelId}/detay")]
        public async Task<ActionResult<PuantajDetayDTO>> GetDetayByTarih(
            int personelId, 
            [FromQuery] DateTime tarih)
        {
            try
            {
                var detay = await _puantajService.GetDetayByTarihAsync(personelId, tarih);
                if (detay == null)
                    return NotFound("Belirtilen tarih için detay bulunamadı.");

                return Ok(detay);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Hata: {ex.Message}");
            }
        }

        // POST: api/Puantaj/rapor/ozet
        [HttpPost("rapor/ozet")]
        public async Task<ActionResult<PuantajOzetRaporDTO>> GetOzetRapor(
            [FromBody] PuantajRaporParametreDTO parametre)
        {
            try
            {
                var rapor = await _puantajService.GetOzetRaporAsync(parametre);
                return Ok(rapor);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Hata: {ex.Message}");
            }
        }

        // GET: api/Puantaj/rapor/departman?yil=2025&ay=1
        [HttpGet("rapor/departman")]
        public async Task<ActionResult<List<DepartmanPuantajOzetDTO>>> GetDepartmanOzet(
            [FromQuery] int yil, 
            [FromQuery] int ay)
        {
            try
            {
                var rapor = await _puantajService.GetDepartmanOzetAsync(yil, ay);
                return Ok(rapor);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Hata: {ex.Message}");
            }
        }

        // GET: api/Puantaj/kontrol?personelId=1&yil=2025&ay=1
        [HttpGet("kontrol")]
        public async Task<ActionResult> PuantajKontrol(
            [FromQuery] int personelId, 
            [FromQuery] int yil, 
            [FromQuery] int ay)
        {
            try
            {
                var varMi = await _puantajService.PuantajVarMiAsync(personelId, yil, ay);
                var hatalar = await _puantajService.ValidasyonKontrolAsync(personelId, yil, ay);

                return Ok(new 
                { 
                    puantajMevcut = varMi,
                    validasyonHatalari = hatalar,
                    hazir = hatalar.Count == 0
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Hata: {ex.Message}");
            }
        }
    }
}
