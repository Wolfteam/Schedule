using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
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
        public HomeController(IOptions<AppSettings> appSettings)
            : base(appSettings)
        {
        }

        public async Task<IActionResult> Index()
        {
            System.Diagnostics.Debug.WriteLine(User.FindFirstValue("access_token"));
            var authenticateInfo = await HttpContext.GetTokenAsync("Bearer");
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