using Microsoft.AspNetCore.Mvc;

namespace PDKS.WebUI.Controllers
{
    public class TatilController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
