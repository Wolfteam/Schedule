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
using Schedule.Web.Models.Repositories;
using Schedule.Web.Helpers;

namespace Schedule.Web.Controllers
{
    [Authorize]
    public class CargarDisponibilidadController : BaseController
    {
        private UnitOfWork _unitOfWork;
        public CargarDisponibilidadController(IOptions<AppSettings> appSettings, IHttpClientsFactory httpClientsFactory)
            : base(appSettings, httpClientsFactory)
        {
            //TODO: TERMINAR ESTO
            _unitOfWork = new UnitOfWork();
        }

        public async Task<ActionResult> Index()
        {
            var httpClient = _httpClientsFactory.GetClient("ScheduleAPI");
            string token = await HttpContext.GetTokenAsync("access_token");
            HttpHelpers.SetHttpClientDefaults(httpClient, token);

            ProfesorRepository pr = new ProfesorRepository(httpClient);
            List<ProfesorDetailsDTO> model = new List<ProfesorDetailsDTO>();

            var claims = User.Claims;
            if (User.IsInRole("Administrador"))
                model = await pr.GetAll();
            else
            {
                int cedula = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var profesor = await pr.Get(cedula);
                model.Add(profesor);
            }
            return View(model.OrderBy(x => x.Nombre));
        }

        [HttpGet("[controller]/{cedula}")]
        public async Task<DisponibilidadProfesorDetailsDTO> Get(int cedula)
        {
            var httpClient = _httpClientsFactory.GetClient("ScheduleAPI");
            string token = await HttpContext.GetTokenAsync("access_token");
            HttpHelpers.SetHttpClientDefaults(httpClient, token);

        }
    }
}
