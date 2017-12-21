using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Schedule.Entities;
using Schedule.Web.ViewModels;

namespace Schedule.Web.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        public HomeController(IOptions<AppSettings> appSettings, IHttpClientsFactory httpClientsFactory)
            : base(appSettings, httpClientsFactory)
        {
        }

        public IActionResult Index()
        {
            HomeViewModel model = new HomeViewModel
            {
                Username = User.Identity.Name,
                UrlPlanificacionAcademica = $"{_appSettings.Value.URLBaseAPI}api/HorarioProfesor/PlanificacionAcademica",
                UrlPlanificacionAulas = $"{_appSettings.Value.URLBaseAPI}api/HorarioProfesor/PlanificacionAulas",
                UrlPlanificacionHorarios = $"{_appSettings.Value.URLBaseAPI}api/HorarioProfesor/PlanificacionHorario"
            };
            return View(model);
        }
    }
}