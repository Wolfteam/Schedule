using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Schedule.Web.Models;
using Schedule.Entities;
using Microsoft.Extensions.Options;
using Schedule.Web.Filters;
using Schedule.Web.ViewModels;

namespace Schedule.Web.Controllers
{
    [AuthenticateAttribute]
    public class HomeController : BaseController
    {
        #region Constructor
        public HomeController(IOptions<AppSettings> appSettings)
            :base(appSettings)
        {
        }
        #endregion

        public IActionResult Index()
        {
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
