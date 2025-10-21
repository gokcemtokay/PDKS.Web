// PDKS.WebUI/Controllers/DashboardController.cs - YENİ

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PDKS.Business.Services;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PDKS.WebUI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        // GET: api/Dashboard/ana
        [HttpGet("ana")]
        public async Task<IActionResult> GetAnaDashboard()
        {
            var kullaniciId = GetKullaniciId();
            var data = await _dashboardService.GetAnaDashboardAsync(kullaniciId);
            return Ok(data);
        }

        // GET: api/Dashboard/manager
        [HttpGet("manager")]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> GetManagerDashboard()
        {
            var kullaniciId = GetKullaniciId();
            var data = await _dashboardService.GetManagerDashboardAsync(kullaniciId);
            return Ok(data);
        }

        // GET: api/Dashboard/ik
        [HttpGet("ik")]
        [Authorize(Roles = "IK,Admin")]
        public async Task<IActionResult> GetIKDashboard()
        {
            var sirketId = GetSirketId();
            var data = await _dashboardService.GetIKDashboardAsync(sirketId);
            return Ok(data);
        }

        // GET: api/Dashboard/executive
        [HttpGet("executive")]
        [Authorize(Roles = "GenelMudur,Admin")]
        public async Task<IActionResult> GetExecutiveDashboard()
        {
            var sirketId = GetSirketId();
            var data = await _dashboardService.GetExecutiveDashboardAsync(sirketId);
            return Ok(data);
        }

        // GET: api/Dashboard/widgets/bugunku-durum
        [HttpGet("widgets/bugunku-durum")]
        public async Task<IActionResult> GetBugunkunDurum()
        {
            var sirketId = GetSirketId();
            var data = await _dashboardService.GetBugunkunDurumAsync(sirketId);
            return Ok(data);
        }

        // GET: api/Dashboard/widgets/bekleyen-onaylar
        [HttpGet("widgets/bekleyen-onaylar")]
        public async Task<IActionResult> GetBekleyenOnaylar()
        {
            var kullaniciId = GetKullaniciId();
            var data = await _dashboardService.GetBekleyenOnaylarWidgetAsync(kullaniciId);
            return Ok(data);
        }

        // GET: api/Dashboard/widgets/son-aktiviteler
        [HttpGet("widgets/son-aktiviteler")]
        public async Task<IActionResult> GetSonAktiviteler()
        {
            var kullaniciId = GetKullaniciId();
            var data = await _dashboardService.GetSonAktivitelerAsync(kullaniciId, 10);
            return Ok(data);
        }

        // GET: api/Dashboard/widgets/dogum-gunleri
        [HttpGet("widgets/dogum-gunleri")]
        public async Task<IActionResult> GetDogumGunleri()
        {
            var sirketId = GetSirketId();
            var data = await _dashboardService.GetDogumGunleriAsync(sirketId);
            return Ok(data);
        }

        // GET: api/Dashboard/widgets/yildonumleri
        [HttpGet("widgets/yildonumleri")]
        public async Task<IActionResult> GetYilDonumleri()
        {
            var sirketId = GetSirketId();
            var data = await _dashboardService.GetYilDonumleriAsync(sirketId);
            return Ok(data);
        }

        private int GetKullaniciId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");
            return claim != null && int.TryParse(claim.Value, out int id) ? id : 0;
        }

        private int GetSirketId()
        {
            var claim = User.FindFirst("sirketId");
            return claim != null && int.TryParse(claim.Value, out int id) ? id : 0;
        }
    }
}