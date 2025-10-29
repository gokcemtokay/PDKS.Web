using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PDKS.Business.DTOs;
using PDKS.Business.Services;

namespace PDKS.WebUI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PersonelOzlukController : ControllerBase
    {
        private readonly IPersonelOzlukService _service;

        public PersonelOzlukController(IPersonelOzlukService service)
        {
            _service = service;
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


        #region Aile Bilgileri

        [HttpGet("aile/{personelId}")]
        public async Task<ActionResult<IEnumerable<PersonelAileDTO>>> GetAileBilgileri(int personelId)
        {
            var result = await _service.GetAileBilgileriAsync(personelId);
            return Ok(result);
        }

        [HttpGet("aile/detay/{id}")]
        public async Task<ActionResult<PersonelAileDTO>> GetAileBilgisiById(int id)
        {
            try
            {
                var result = await _service.GetAileBilgisiByIdAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost("aile")]
        public async Task<ActionResult<PersonelAileDTO>> CreateAileBilgisi([FromBody] PersonelAileCreateDTO dto)
        {
            var result = await _service.CreateAileBilgisiAsync(dto);
            return CreatedAtAction(nameof(GetAileBilgisiById), new { id = result.Id }, result);
        }

        [HttpPut("aile/{id}")]
        public async Task<ActionResult> UpdateAileBilgisi(int id, [FromBody] PersonelAileCreateDTO dto)
        {
            var result = await _service.UpdateAileBilgisiAsync(id, dto);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("aile/{id}")]
        public async Task<ActionResult> DeleteAileBilgisi(int id)
        {
            var result = await _service.DeleteAileBilgisiAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        #endregion

        #region Acil Durum

        [HttpGet("acildurum/{personelId}")]
        public async Task<ActionResult<IEnumerable<PersonelAcilDurumDTO>>> GetAcilDurumBilgileri(int personelId)
        {
            var result = await _service.GetAcilDurumBilgileriAsync(personelId);
            return Ok(result);
        }

        [HttpGet("acildurum/detay/{id}")]
        public async Task<ActionResult<PersonelAcilDurumDTO>> GetAcilDurumById(int id)
        {
            try
            {
                var result = await _service.GetAcilDurumByIdAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost("acildurum")]
        public async Task<ActionResult<PersonelAcilDurumDTO>> CreateAcilDurum([FromBody] PersonelAcilDurumCreateDTO dto)
        {
            var result = await _service.CreateAcilDurumAsync(dto);
            return CreatedAtAction(nameof(GetAcilDurumById), new { id = result.Id }, result);
        }

        [HttpPut("acildurum/{id}")]
        public async Task<ActionResult> UpdateAcilDurum(int id, [FromBody] PersonelAcilDurumCreateDTO dto)
        {
            var result = await _service.UpdateAcilDurumAsync(id, dto);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("acildurum/{id}")]
        public async Task<ActionResult> DeleteAcilDurum(int id)
        {
            var result = await _service.DeleteAcilDurumAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        #endregion

        #region Sağlık

        [HttpGet("saglik/{personelId}")]
        public async Task<ActionResult<PersonelSaglikDTO>> GetSaglikBilgisi(int personelId)
        {
            var result = await _service.GetSaglikBilgisiAsync(personelId);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost("saglik")]
        public async Task<ActionResult<PersonelSaglikDTO>> CreateOrUpdateSaglikBilgisi([FromBody] PersonelSaglikDTO dto)
        {
            var result = await _service.CreateOrUpdateSaglikBilgisiAsync(dto);
            return Ok(result);
        }

        #endregion

        #region Eğitim Geçmişi

        [HttpGet("egitim/{personelId}")]
        public async Task<ActionResult<IEnumerable<PersonelEgitimDTO>>> GetEgitimGecmisi(int personelId)
        {
            var result = await _service.GetEgitimGecmisiAsync(personelId);
            return Ok(result);
        }

        [HttpGet("egitim/detay/{id}")]
        public async Task<ActionResult<PersonelEgitimDTO>> GetEgitimById(int id)
        {
            try
            {
                var result = await _service.GetEgitimByIdAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost("egitim")]
        public async Task<ActionResult<PersonelEgitimDTO>> CreateEgitim([FromBody] PersonelEgitimCreateDTO dto)
        {
            var result = await _service.CreateEgitimAsync(dto);
            return CreatedAtAction(nameof(GetEgitimById), new { id = result.Id }, result);
        }

        [HttpPut("egitim/{id}")]
        public async Task<ActionResult> UpdateEgitim(int id, [FromBody] PersonelEgitimCreateDTO dto)
        {
            var result = await _service.UpdateEgitimAsync(id, dto);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("egitim/{id}")]
        public async Task<ActionResult> DeleteEgitim(int id)
        {
            var result = await _service.DeleteEgitimAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        #endregion

        #region Sertifikalar

        [HttpGet("sertifika/{personelId}")]
        public async Task<ActionResult<IEnumerable<PersonelSertifikaDTO>>> GetSertifikalar(int personelId)
        {
            var result = await _service.GetSertifikalarAsync(personelId);
            return Ok(result);
        }

        [HttpGet("sertifika/expiring")]
        public async Task<ActionResult<IEnumerable<PersonelSertifikaDTO>>> GetExpiringSertifikalar([FromQuery] int days = 30)
        {
            var result = await _service.GetExpiringSertifikalarAsync(days);
            return Ok(result);
        }

        [HttpGet("sertifika/expired")]
        public async Task<ActionResult<IEnumerable<PersonelSertifikaDTO>>> GetExpiredSertifikalar()
        {
            var result = await _service.GetExpiredSertifikalarAsync();
            return Ok(result);
        }

        [HttpPost("sertifika")]
        public async Task<ActionResult<PersonelSertifikaDTO>> CreateSertifika([FromBody] PersonelSertifikaCreateDTO dto)
        {
            var result = await _service.CreateSertifikaAsync(dto);
            return Ok(result);
        }

        [HttpPut("sertifika/{id}")]
        public async Task<ActionResult> UpdateSertifika(int id, [FromBody] PersonelSertifikaCreateDTO dto)
        {
            var result = await _service.UpdateSertifikaAsync(id, dto);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("sertifika/{id}")]
        public async Task<ActionResult> DeleteSertifika(int id)
        {
            var result = await _service.DeleteSertifikaAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        #endregion

        #region Performans

        [HttpGet("performans/{personelId}")]
        public async Task<ActionResult<IEnumerable<PersonelPerformansDTO>>> GetPerformansKayitlari(int personelId)
        {
            var result = await _service.GetPerformansKayitlariAsync(personelId);
            return Ok(result);
        }

        [HttpPost("performans")]
        public async Task<ActionResult<PersonelPerformansDTO>> CreatePerformans([FromBody] PersonelPerformansCreateDTO dto)
        {
            var result = await _service.CreatePerformansAsync(dto);
            return Ok(result);
        }

        [HttpPost("performans/{id}/onayla")]
        public async Task<ActionResult> OnaylaPerformans(int id, [FromBody] int onaylayanKullaniciId)
        {
            var result = await _service.OnaylaPerformansAsync(id, onaylayanKullaniciId);
            if (!result) return NotFound();
            return NoContent();
        }

        #endregion

        #region Zimmet

        [HttpGet("zimmet/{personelId}")]
        public async Task<ActionResult<IEnumerable<PersonelZimmetDTO>>> GetZimmetler(int personelId)
        {
            var result = await _service.GetZimmetlerAsync(personelId);
            return Ok(result);
        }

        [HttpPost("zimmet")]
        public async Task<ActionResult<PersonelZimmetDTO>> CreateZimmet([FromBody] PersonelZimmetCreateDTO dto)
        {
            var result = await _service.CreateZimmetAsync(dto);
            return Ok(result);
        }

        [HttpPost("zimmet/{id}/iade")]
        public async Task<ActionResult> IadeZimmet(int id, [FromBody] ZimmetIadeDTO dto)
        {
            var result = await _service.IadeZimmetAsync(id, dto.IadeTeslimAlanKullaniciId, dto.IadeTarihi);
            if (!result) return NotFound();
            return NoContent();
        }

        #endregion

        #region Kombine Özlük Detay

        [HttpGet("{personelId}/ozluk-detay")]
        public async Task<ActionResult<PersonelOzlukDetayDTO>> GetPersonelOzlukDetay(int personelId)
        {
            var result = await _service.GetPersonelOzlukDetayAsync(personelId);
            return Ok(result);
        }

        #endregion

        // Diğer endpoint'ler için benzer pattern kullanılabilir
        // İş deneyimi, dil, disiplin, terfi, ücret değişiklik, referans, yetkinlik, eğitim kayıt, mali bilgi, ek bilgi
    }

    // Helper DTO for Zimmet İade
    public class ZimmetIadeDTO
    {
        public int IadeTeslimAlanKullaniciId { get; set; }
        public DateTime IadeTarihi { get; set; }
    }
}
