using Microsoft.AspNetCore.Mvc;
using Schedule.API.Models.Repositories;

namespace Schedule.API.Controllers
{
    [Route("api/[controller]")]
    public class BaseController : Controller
    {
        protected readonly UnitOfWork _db = new UnitOfWork();

        // public BaseController()
        // {
        // }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}