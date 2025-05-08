using Microsoft.AspNetCore.Mvc;

namespace otoSignalR.Controllers
{
    public class SignalRController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
