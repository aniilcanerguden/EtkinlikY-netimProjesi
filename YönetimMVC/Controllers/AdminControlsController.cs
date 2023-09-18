using Microsoft.AspNetCore.Mvc;

namespace YönetimMVC.Controllers
{
    public class AdminControlsController : Controller
    {
        public IActionResult Event()
        {
            return View();
        }
        public IActionResult CancelEvent()
        {
            return View();
        }
        public IActionResult ApproveEvent()
        {
            return View();
        }
        public IActionResult AddCity()
        {
            return View();
        }
        public IActionResult Category()
        {
            return View();
        }
    }
}
