using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Schedule.Entities;
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
        public CargarDisponibilidadController(IOptions<AppSettings> appSettings, IHttpClientsFactory httpClientsFactory)
            : base(appSettings, httpClientsFactory)
        {
        }

        public async Task<ActionResult> Index()
        {
            var httpClient =_httpClientsFactory.GetClient("ScheduleAPI");
            string token = await HttpContext.GetTokenAsync("access_token");

            ProfesorRepository pr = new ProfesorRepository(httpClient, token);
            List<ProfesorDetailsDTO> model = new List<ProfesorDetailsDTO>();

            var claims = User.Claims;
            if (User.IsInRole("Administrador"))
                model = await pr.GetAll();
            else
            {
                //TODO: Aca me daba error xq hay algo raro en NameIdentifier
                var xd = User.FindFirstValue(ClaimTypes.NameIdentifier);
                int cedula = Int32.Parse(xd);
                var profesor = await pr.Get(cedula);
                model.Add(profesor);
            }
            return View(model.OrderBy(x => x.Nombre));
        }
    }
}
