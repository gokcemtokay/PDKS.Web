using Microsoft.AspNetCore.Mvc;

namespace PDKS.WebUI.Controllers
{
    public class KullaniciController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
