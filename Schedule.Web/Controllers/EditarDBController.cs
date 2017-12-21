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
        public EditarDBController(IOptions<AppSettings> appSettings, IHttpClientsFactory httpClientsFactory)
            : base(appSettings, httpClientsFactory)
        {
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}