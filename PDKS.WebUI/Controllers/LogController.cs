using Microsoft.AspNetCore.Mvc;

namespace PDKS.WebUI.Controllers
{
    public class LogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
