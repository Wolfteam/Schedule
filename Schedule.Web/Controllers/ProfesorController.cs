using Microsoft.AspNetCore.Mvc;
using Schedule.DAO;
using Schedule.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Schedule.Web.Controllers
{
    public class ProfesorController : Controller
    {
  
        public ProfesorController()
        {
         
        }

        public IActionResult Index()
        {
            DBConnection db = new DBConnection();

            return View();
        }
    }
}
