using Microsoft.AspNetCore.Mvc;
using Schedule.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Schedule.Web.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AuthenticateUser(string username, string password)
        {
            bool result = new UsuarioBLL().AuthenticateUser(username, password);
            if (!result)
            {
                return RedirectToAction("Index","Login");
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
