using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PDKS.Business.DTOs;
using PDKS.Business.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PDKS.WebUI.Controllers
{
    [Authorize] // Bu controller'a erişim için yetkilendirme (geçerli token) gerekir.
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class PersonelController : ControllerBase
    {
        private readonly IPersonelService _personelService;
        private readonly IDepartmanService _departmanService;
        private readonly IVardiyaService _vardiyaService;
        private readonly ISirketService _sirketService;

        // BaseController'dan gelen IUnitOfWork bağımlılığına artık burada ihtiyacımız yok,
        // çünkü loglama gibi işlemler doğrudan servisler içinde yapılabilir veya daha merkezi bir yerden yönetilebilir.
        public PersonelController(
            IPersonelService personelService,
            IDepartmanService departmanService,
            IVardiyaService vardiyaService,
            ISirketService sirketService)
        {
            _personelService = personelService;
            _departmanService = departmanService;
            _vardiyaService = vardiyaService;
            _sirketService = sirketService;
        }

        // GET: api/Personel
        [HttpGet]
        public async Task<IActionResult> GetPersoneller()
        {
            try
            {
                // TODO: Şirket ID'sini JWT Token içerisinden alacak şekilde güncellenebilir.
                // Şimdilik tüm personelleri listeleyelim.
                var personeller = await _personelService.GetAllAsync();
                return Ok(personeller);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // GET: api/Personel/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPersonelById(int id)
        {
            try
            {
                var personel = await _personelService.GetByIdAsync(id);
                if (personel == null)
                {
                    return NotFound($"Personel with ID {id} not found.");
                }
                return Ok(personel);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // POST: api/Personel
        [HttpPost]
        [Authorize(Roles = "Admin,IK")] // Sadece Admin veya IK rolündeki kullanıcılar personel ekleyebilir.
        public async Task<IActionResult> CreatePersonel([FromBody] PersonelCreateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var newPersonelId = await _personelService.CreateAsync(dto);
                var createdPersonel = await _personelService.GetByIdAsync(newPersonelId);
                return CreatedAtAction(nameof(GetPersonelById), new { id = newPersonelId }, createdPersonel);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // PUT: api/Personel/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,IK")] // Sadece Admin veya IK rolündeki kullanıcılar personel güncelleyebilir.
        public async Task<IActionResult> UpdatePersonel(int id, [FromBody] PersonelUpdateDTO dto)
        {
            if (id != dto.Id)
            {
                return BadRequest("Personel ID mismatch.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _personelService.UpdateAsync(dto);
                return NoContent(); // Başarılı güncellemede 204 No Content döndürülür.
            }
            catch (Exception ex)
            {
                // Personel bulunamadı hatası servisten gelebilir, bunu daha spesifik yakalayabiliriz.
                if (ex.Message.Contains("bulunamadı"))
                {
                    return NotFound(ex.Message);
                }
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // DELETE: api/Personel/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // Sadece Admin rolündeki kullanıcılar personel silebilir.
        public async Task<IActionResult> DeletePersonel(int id)
        {
            try
            {
                await _personelService.DeleteAsync(id);
                return NoContent(); // Başarılı silme işleminde 204 No Content döndürülür.
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("bulunamadı"))
                {
                    return NotFound(ex.Message);
                }
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}