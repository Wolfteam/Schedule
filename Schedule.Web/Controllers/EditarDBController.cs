using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Schedule.Entities;
using Schedule.Web.Filters;

namespace Schedule.Web.Controllers
{
    [AuthenticateAttribute]
    public class EditarDBController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}