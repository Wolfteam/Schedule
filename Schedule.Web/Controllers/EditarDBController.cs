using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Schedule.Entities;
using Schedule.Web.Filters;

namespace Schedule.Web.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class EditarDBController : BaseController
    {
        public EditarDBController(IOptions<AppSettings> appSettings)
            : base(appSettings)
        {
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}