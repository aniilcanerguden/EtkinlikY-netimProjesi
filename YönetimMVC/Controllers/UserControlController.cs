using EtkinlikYönetimProjesi.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace YönetimMVC.Controllers
{
    public class UserControlController : Controller
    {
        
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult EventList()
        {
    

            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult CreateEvent()
        {
            return View();
        }
        public IActionResult Edit()
        {
            return View();
        }
        public IActionResult Cancel()
        {
            return View();

        }
        public IActionResult UserInformations()
        {
            return View();
        }
        public IActionResult GetJoiningEvent()
        {
            return View();
        }

    }
}
