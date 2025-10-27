using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PDKS.Business.DTOs;
using PDKS.Business.Services;

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
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var menuler = await _menuService.GetAllAsync();
            return Ok(menuler);
        }

        [HttpGet("ana-menuler")]
        public async Task<IActionResult> GetAnaMenuler()
        {
            var menuler = await _menuService.GetAnaMenulerAsync();
            return Ok(menuler);
        }

        [HttpGet("rol/{rolId}")]
        public async Task<IActionResult> GetMenulerByRol(int rolId)
        {
            var menuler = await _menuService.GetMenulerByRolIdAsync(rolId);
            return Ok(menuler);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var menu = await _menuService.GetByIdAsync(id);
            if (menu == null)
                return NotFound();

            return Ok(menu);
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Create([FromBody] MenuDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var menu = await _menuService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = menu.Id }, menu);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Update(int id, [FromBody] MenuDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var menu = await _menuService.UpdateAsync(id, dto);
                return Ok(menu);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _menuService.DeleteAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
