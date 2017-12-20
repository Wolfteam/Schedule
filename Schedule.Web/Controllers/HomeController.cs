using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Schedule.Web.Models;
using Schedule.Entities;
using Microsoft.Extensions.Options;
using Schedule.Web.Filters;
using Schedule.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Schedule.Web.Controllers
{
    [Authorize(Roles ="Profesor")]
    public class HomeController : Controller
    {
        IOptions<AppSettings> _appSettings;
        #region Constructor
        public HomeController(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings;
        }
        #endregion

        public IActionResult Index()
        {
            foreach (var claim in User.Claims)
            {
                Debug.WriteLine("Type:"+ claim.Type + ",  value:" +claim.Value);
            }
            HomeViewModel model = new HomeViewModel
            {
                UrlPlanificacionAcademica = $"{_appSettings.Value.URLBaseAPI}api/HorarioProfesor/PlanificacionAcademica",
                UrlPlanificacionAulas = $"{_appSettings.Value.URLBaseAPI}api/HorarioProfesor/PlanificacionAulas",
                UrlPlanificacionHorarios = $"{_appSettings.Value.URLBaseAPI}api/HorarioProfesor/PlanificacionHorario"
            };
            return View(model);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
