using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PDKS.Business.DTOs;
using PDKS.Business.Services;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PDKS.WebUI.Controllers
{
    [Authorize] // Bu controller'a erişim için yetkilendirme (geçerli token) gerekir.
    [ApiController]
#if DEBUG
    [Route("api/[controller]")] // ⬅️ Development: /api/auth/login
#else
[Route("[controller]")] // ⬅️ Production: /auth/login (IIS /api ekler)
#endif
    [Produces("application/json")]
    [Consumes("application/json")]
    public class PersonelController : ControllerBase
    {
        private readonly IPersonelService _personelService;
        private readonly IDepartmanService _departmanService;
        private readonly IVardiyaService _vardiyaService;
        private readonly ISirketService _sirketService;

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

        // Yardımcı metot: JWT token'dan aktif şirket ID'sini alır.
        private int GetCurrentSirketId()
        {
            var sirketIdClaim = User.Claims.FirstOrDefault(c => c.Type == "sirketId");
            if (sirketIdClaim != null && int.TryParse(sirketIdClaim.Value, out int sirketId))
            {
                return sirketId;
            }
            // Token geçerli olduğu varsayılır, ancak sirketId yoksa hata fırlatılır.
            throw new UnauthorizedAccessException("Yetkilendirme token'ında şirket ID'si bulunamadı.");
        }


        // GET: api/Personel
        [HttpGet]
        public async Task<IActionResult> GetPersoneller()
        {
            try
            {
                // ⭐ GÜNCELLEME: JWT Token'dan şirket ID'sini al ve filtrele
                var sirketId = GetCurrentSirketId();
                // Bu çağrının Servis katmanında Personel.SirketId == sirketId filtresi yapması beklenir.
                var personeller = await _personelService.GetBySirketAsync(sirketId);

                return Ok(personeller);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
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

                // ⭐ GÜVENLİK KONTROLÜ: Personelin aktif şirkete ait olup olmadığını kontrol et
                // Not: Servis'ten dönen DTO'nun SirketId içermesi gerekir.
                // Assuming PersonelDetailDTO/PersonelListDTO has SirketId property.
                var sirketId = GetCurrentSirketId();
                if (personel.SirketId != sirketId)
                {
                    return Forbid("Bu personel, yetkili olduğunuz şirkete ait değildir.");
                }

                return Ok(personel);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // POST: api/Personel
        [HttpPost]
        [Authorize(Roles = "Admin,IK")]
        public async Task<IActionResult> CreatePersonel([FromBody] PersonelCreateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // ⭐ GÜVENLİK KONTROLÜ: DTO'daki şirket ID'si token'daki ID ile eşleşmeli
                var sirketId = GetCurrentSirketId();
                if (dto.SirketId != sirketId)
                {
                    return Forbid("Yeni personel kaydı, sadece aktif şirketinize yapılabilir.");
                }

                var newPersonelId = await _personelService.CreateAsync(dto);
                var createdPersonel = await _personelService.GetByIdAsync(newPersonelId);
                return CreatedAtAction(nameof(GetPersonelById), new { id = newPersonelId }, createdPersonel);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // PUT: api/Personel/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,IK")]
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
                // ⭐ GÜVENLİK KONTROLÜ: DTO'daki şirket ID'si token'daki ID ile eşleşmeli
                var sirketId = GetCurrentSirketId();
                if (dto.SirketId != sirketId)
                {
                    return Forbid("Personel güncellemesi, sadece aktif şirketinize yapılabilir.");
                }

                await _personelService.UpdateAsync(dto);
                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
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

        // DELETE: api/Personel/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePersonel(int id)
        {
            try
            {
                // Personelin SirketId'sini öğrenmek için PersonelService'ten çekilir
                var personel = await _personelService.GetByIdAsync(id);
                if (personel == null)
                {
                    return NotFound($"Personel with ID {id} not found.");
                }

                // ⭐ GÜVENLİK KONTROLÜ: Personelin aktif şirkete ait olup olmadığını kontrol et
                var sirketId = GetCurrentSirketId();
                if (personel.SirketId != sirketId)
                {
                    return Forbid("Bu personel, yetkili olduğunuz şirkete ait değildir ve silinemez.");
                }

                await _personelService.DeleteAsync(id);
                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
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

        [HttpPost("{id}/foto")]
        [Authorize(Roles = "Admin,IK")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadFoto(int id, IFormFile file)
        {
            try
            {
                // ✅ LOG: Başlangıç
                Console.WriteLine($"=== UploadFoto START === ID: {id}");

                if (file == null || file.Length == 0)
                    return BadRequest(new { message = "Dosya seçilmedi" });

                Console.WriteLine($"File: {file.FileName}, Size: {file.Length} bytes");

                // Personel kontrolü
                var personel = await _personelService.GetByIdAsync(id);
                if (personel == null)
                    return NotFound(new { message = "Personel bulunamadı" });

                // Şirket kontrolü
                var sirketId = GetCurrentSirketId();
                if (personel.SirketId != sirketId)
                    return Forbid("Bu personel, yetkili olduğunuz şirkete ait değildir.");

                // Dosya uzantısı kontrolü
                var extension = Path.GetExtension(file.FileName).ToLower();
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                if (!allowedExtensions.Contains(extension))
                    return BadRequest(new { message = "Sadece JPG, PNG ve GIF formatları desteklenir" });

                // Dosya boyutu kontrolü (5MB)
                if (file.Length > 5 * 1024 * 1024)
                    return BadRequest(new { message = "Dosya boyutu 5MB'dan büyük olamaz" });

                // Uploads klasörü
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "personel");
                if (!Directory.Exists(uploadsFolder))
                {
                    Console.WriteLine($"Creating directory: {uploadsFolder}");
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Eski fotoğrafı sil (varsa)
                if (!string.IsNullOrEmpty(personel.ProfilResmi))
                {
                    var oldPhotoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", personel.ProfilResmi.TrimStart('/'));
                    if (System.IO.File.Exists(oldPhotoPath))
                    {
                        Console.WriteLine($"Deleting old photo: {oldPhotoPath}");
                        System.IO.File.Delete(oldPhotoPath);
                    }
                }

                // Yeni dosya adı (unique)
                var fileName = $"personel_{id}_{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                // Dosyayı kaydet
                Console.WriteLine($"Saving file to: {filePath}");
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                Console.WriteLine("File saved successfully!");

                // ✅ ÖNEMLİ: Database'de SADECE ProfilResmi alanını güncelle
                var photoUrl = $"/uploads/personel/{fileName}";
                Console.WriteLine($"Updating database: ProfilResmi = {photoUrl}");

                await _personelService.UpdateProfilFotoAsync(id, photoUrl);

                Console.WriteLine("Database updated successfully!");
                Console.WriteLine($"=== UploadFoto SUCCESS === URL: {photoUrl}");

                return Ok(new
                {
                    message = "Fotoğraf başarıyla yüklendi",
                    foto = photoUrl,
                    profilResmi = photoUrl
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"=== UploadFoto ERROR ===");
                Console.WriteLine($"Message: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"InnerException: {ex.InnerException.Message}");
                }
                return StatusCode(500, new { message = "Fotoğraf yüklenirken hata oluştu", error = ex.Message });
            }
        }

        [HttpDelete("{id}/foto")]
        [Authorize(Roles = "Admin,IK")]
        public async Task<IActionResult> DeleteFoto(int id)
        {
            try
            {
                var personel = await _personelService.GetByIdAsync(id);
                if (personel == null)
                    return NotFound(new { message = "Personel bulunamadı" });

                var sirketId = GetCurrentSirketId();
                if (personel.SirketId != sirketId)
                    return Forbid("Bu personel, yetkili olduğunuz şirkete ait değildir.");

                if (string.IsNullOrEmpty(personel.ProfilResmi))
                    return BadRequest(new { message = "Silinecek fotoğraf bulunamadı" });

                // Fiziksel dosyayı sil
                var photoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", personel.ProfilResmi.TrimStart('/'));
                if (System.IO.File.Exists(photoPath))
                    System.IO.File.Delete(photoPath);

                // ✅ BASITLEŞTIRILMIŞ: Sadece fotoğraf URL'ini temizle
                await _personelService.UpdateProfilFotoAsync(id, null);

                return Ok(new { message = "Fotoğraf başarıyla silindi" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Fotoğraf silinirken hata oluştu", error = ex.Message });
            }
        }

        [HttpGet("{id}/foto")]
        [AllowAnonymous] // Fotoğraf görüntüleme için yetki gerekmeyebilir
        public async Task<IActionResult> GetFoto(int id)
        {
            try
            {
                // Personel kontrolü - SERVİS KULLAN
                var personel = await _personelService.GetByIdAsync(id);
                if (personel == null)
                    return NotFound(new { message = "Personel bulunamadı" });

                if (string.IsNullOrEmpty(personel.ProfilResmi))
                    return NotFound(new { message = "Personelin fotoğrafı yok" });

                var photoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", personel.ProfilResmi.TrimStart('/'));
                if (!System.IO.File.Exists(photoPath))
                    return NotFound(new { message = "Fotoğraf dosyası bulunamadı" });

                var extension = Path.GetExtension(photoPath).ToLower();
                var contentType = extension switch
                {
                    ".jpg" => "image/jpeg",
                    ".jpeg" => "image/jpeg",
                    ".png" => "image/png",
                    ".gif" => "image/gif",
                    _ => "application/octet-stream"
                };

                var fileBytes = await System.IO.File.ReadAllBytesAsync(photoPath);
                return File(fileBytes, contentType);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Fotoğraf yüklenirken hata oluştu", error = ex.Message });
            }
        }

    }
}