using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PDKS.Business.DTOs;
using PDKS.Business.Services;
using PDKS.Data.Entities;
using PDKS.Data.Repositories;
using System.Security.Claims;

namespace PDKS.WebUI.Controllers
{
    [Authorize]
    [ApiController]
#if DEBUG
    [Route("api/[controller]")]
#else
    [Route("[controller]")]
#endif
    [Produces("application/json")]
    [Consumes("application/json")]
    public class RolYetkiController : ControllerBase
    {
        private readonly IRolYetkiService _rolYetkiService;
        private readonly IUnitOfWork _unitOfWork;

        public RolYetkiController(IRolYetkiService rolYetkiService, IUnitOfWork unitOfWork)
        {
            _rolYetkiService = rolYetkiService;
            _unitOfWork = unitOfWork;
        }

        #region ROL CRUD İŞLEMLERİ

        // GET: api/RolYetki
        [HttpGet]
        public async Task<IActionResult> GetAllRoller()
        {
            var roller = await _unitOfWork.Roller.GetAllAsync();
            var result = roller.Select(r => new
            {
                id = r.Id,
                rolAdi = r.RolAdi,
                aciklama = r.Aciklama,
                aktif = r.Aktif
            });
            return Ok(result);
        }

        // GET: api/RolYetki/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRolById(int id)
        {
            var rol = await _unitOfWork.Roller.GetByIdAsync(id);
            if (rol == null)
                return NotFound();

            return Ok(new
            {
                id = rol.Id,
                rolAdi = rol.RolAdi,
                aciklama = rol.Aciklama,
                aktif = rol.Aktif
            });
        }

        // POST: api/RolYetki
        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> CreateRol([FromBody] RolCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var rol = new Rol
            {
                RolAdi = dto.RolAdi,
                Aciklama = dto.Aciklama,
                Aktif = dto.Aktif
            };

            await _unitOfWork.Roller.AddAsync(rol);
            await _unitOfWork.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRolById), new { id = rol.Id }, new
            {
                id = rol.Id,
                rolAdi = rol.RolAdi,
                aciklama = rol.Aciklama,
                aktif = rol.Aktif
            });
        }

        // PUT: api/RolYetki/5
        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> UpdateRol(int id, [FromBody] RolUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var rol = await _unitOfWork.Roller.GetByIdAsync(id);
            if (rol == null)
                return NotFound();

            rol.RolAdi = dto.RolAdi;
            rol.Aciklama = dto.Aciklama;
            rol.Aktif = dto.Aktif;

            await _unitOfWork.SaveChangesAsync();

            return Ok(new
            {
                id = rol.Id,
                rolAdi = rol.RolAdi,
                aciklama = rol.Aciklama,
                aktif = rol.Aktif
            });
        }

        // DELETE: api/RolYetki/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteRol(int id)
        {
            var rol = await _unitOfWork.Roller.GetByIdAsync(id);
            if (rol == null)
                return NotFound();

            // Kullanıcısı olan rol silinemez
            var kullaniciSayisi = await _unitOfWork.Kullanicilar
                .CountAsync(k => k.RolId == id);

            if (kullaniciSayisi > 0)
            {
                return BadRequest(new { message = $"Bu role ait {kullaniciSayisi} kullanıcı var. Önce kullanıcıları başka role taşıyın." });
            }

            _unitOfWork.Roller.Remove(rol);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/RolYetki/5/yetkiler
        [HttpGet("{id}/yetkiler")]
        public async Task<IActionResult> GetRolYetkileriDetay(int id)
        {
            var rol = await _unitOfWork.Roller.GetByIdAsync(id);
            if (rol == null)
                return NotFound();

            // Menü yetkileri
            var menuYetkileri = await _unitOfWork.MenuRoller.GetByRolIdAsync(id);
            var menuler = menuYetkileri.Select(mr => new
            {
                menuId = mr.MenuId,
                menuAdi = mr.Menu.MenuAdi,
                okuma = mr.Okuma
            });

            // İşlem yetkileri
            var islemYetkileri = await _unitOfWork.RolIslemYetkiler.GetByRolIdAsync(id);
            var islemler = islemYetkileri.Select(ri => new
            {
                islemYetkiId = ri.IslemYetkiId,
                islemKodu = ri.IslemYetki.IslemKodu,
                islemAdi = ri.IslemYetki.IslemAdi,
                izinli = ri.Izinli
            });

            return Ok(new
            {
                rolId = id,
                rolAdi = rol.RolAdi,
                menuler = menuler,
                islemler = islemler
            });
        }

        #endregion

        #region YETKİLENDİRME İŞLEMLERİ

        // Kullanıcının yetkilerini getir (menü ve işlemler)
        [HttpGet("kullanici-yetkileri")]
        public async Task<IActionResult> GetKullaniciYetkileri()
        {
            var kullaniciIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (kullaniciIdClaim == null || !int.TryParse(kullaniciIdClaim.Value, out int kullaniciId))
            {
                return Unauthorized();
            }

            try
            {
                var yetkiler = await _rolYetkiService.GetKullaniciYetkileriAsync(kullaniciId);
                return Ok(yetkiler);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // Belirli bir rolün yetkilerini getir
        [HttpGet("rol/{rolId}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetRolYetkileri(int rolId)
        {
            var yetkiler = await _rolYetkiService.GetRolYetkileriAsync(rolId);
            return Ok(yetkiler);
        }

        // Role yetki ata
        [HttpPost("ata")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> RolYetkiAta([FromBody] RolYetkiAtamaDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _rolYetkiService.RolYetkiAtaAsync(dto);
            if (result)
                return Ok(new { message = "Yetkilendirme başarılı" });

            return BadRequest(new { message = "Yetkilendirme başarısız" });
        }

        // İşlem yetkisi kontrolü
        [HttpGet("kontrol/{islemKodu}")]
        public async Task<IActionResult> HasPermission(string islemKodu)
        {
            var kullaniciIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (kullaniciIdClaim == null || !int.TryParse(kullaniciIdClaim.Value, out int kullaniciId))
            {
                return Unauthorized();
            }

            var hasPermission = await _rolYetkiService.HasPermissionAsync(kullaniciId, islemKodu);
            return Ok(new { hasPermission });
        }

        // Tüm işlem yetkilerini listele
        [HttpGet("islem-yetkileri")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetAllIslemYetkileri()
        {
            var yetkiler = await _rolYetkiService.GetAllIslemYetkileriAsync();
            return Ok(yetkiler);
        }

        // Yeni işlem yetkisi ekle
        [HttpPost("islem-yetki")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> CreateIslemYetki([FromBody] IslemYetkiDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var yetki = await _rolYetkiService.CreateIslemYetkiAsync(dto);
            return CreatedAtAction(nameof(GetAllIslemYetkileri), new { id = yetki.Id }, yetki);
        }

        #endregion
    }

    #region DTO'LAR

    public class RolCreateDto
    {
        public string RolAdi { get; set; }
        public string? Aciklama { get; set; }
        public bool Aktif { get; set; } = true;
    }

    public class RolUpdateDto
    {
        public string RolAdi { get; set; }
        public string? Aciklama { get; set; }
        public bool Aktif { get; set; }
    }

    #endregion
}
