using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Schedule.Entities;
using Schedule.Web.Models;
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
        private readonly UnitOfWork _unitOfWork;
        public CargarDisponibilidadController(IOptions<AppSettings> appSettings, IHttpClientsFactory httpClientsFactory)
            : base(appSettings, httpClientsFactory)
        {
            _unitOfWork = new UnitOfWork(_httpClientsFactory);
        }

        public async Task<ActionResult> Index()
        {
            _unitOfWork.Token = await HttpContext.GetTokenAsync(_tokenName);
            List<ProfesorDetailsDTO> model = new List<ProfesorDetailsDTO>();
            var claims = User.Claims;
            if (User.IsInRole("Administrador"))
                model = await _unitOfWork.ProfesorRepository.GetAllAsync();
            else
            {
                int cedula = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var profesor = await _unitOfWork.ProfesorRepository.GetAsync(cedula);
                model.Add(profesor);
            }
            return View(model.OrderBy(x => x.Nombre));
        }
    }
}