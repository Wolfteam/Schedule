using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Schedule.Entities;
using Schedule.Web.Filters;
using Schedule.Web.Helpers;
using Schedule.Web.Models.Repository;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace Schedule.Web.Controllers
{
    [Authorize]
    public class CargarDisponibilidadController : BaseController
    {
        public CargarDisponibilidadController(IOptions<AppSettings> appSettings)
            : base(appSettings)
        {

        }

        public async Task<ActionResult> Index()
        {
            ProfesorRepository pr = new ProfesorRepository(_httpClient);
            System.Diagnostics.Debug.WriteLine(_httpClient.DefaultRequestHeaders.Authorization);
            List<ProfesorDetailsDTO> model = new List<ProfesorDetailsDTO>();

            var claims = User.Claims;
            if (User.IsInRole("Administrador"))
                model = await pr.GetAll();
            else
            {
                var xd = User.FindFirstValue(ClaimTypes.NameIdentifier);
                int cedula = Int32.Parse(xd);
                var profesor = await pr.Get(cedula);
                model.Add(profesor);
            }
            return View(model.OrderBy(x => x.Nombre).ToList());
        }
    }
}
