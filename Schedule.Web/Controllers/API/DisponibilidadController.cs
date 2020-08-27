using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Schedule.Entities;
using Schedule.Web.Controllers;
using Schedule.Web.Models;
using Schedule.Web.Models.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Schedule.Web.Controllers.API
{
    [Route("api/[controller]")]
    [Authorize]
    public class DisponibilidadController : BaseController
    {
        private readonly UnitOfWork _unitOfWork;
        public DisponibilidadController(IOptions<AppSettings> appSettings, IHttpClientsFactory httpClientsFactory)
            : base(appSettings, httpClientsFactory)
        {
            _unitOfWork = new UnitOfWork(httpClientsFactory);
        }

        [HttpPost]
        public async Task<IActionResult> AddRangeAsync(List<DisponibilidadProfesorDTO> disponibilidad)
        {
            _unitOfWork.Token = await HttpContext.GetTokenAsync(_tokenName);
            bool result = await _unitOfWork.DisponibilidadRepository.AddRangeAsync(disponibilidad);
            if (!result)
                return StatusCode(500);
            return CreatedAtRoute("GetDisponibilidad", new { cedula = disponibilidad.FirstOrDefault().Cedula }, disponibilidad);
        }

        [HttpGet("{cedula}", Name = "GetDisponibilidad")]
        public async Task<DisponibilidadProfesorDetailsDTO> GetAsync(int cedula)
        {
            _unitOfWork.Token = await HttpContext.GetTokenAsync(_tokenName);
            return await _unitOfWork.DisponibilidadRepository.GetAsync(cedula);
        }

        [HttpDelete("{cedula}")]
        public async Task<IActionResult> RemoveAsync(int cedula)
        {
            _unitOfWork.Token = await HttpContext.GetTokenAsync(_tokenName);
            bool result = await _unitOfWork.AulasRepository.RemoveAsync(cedula);
            if (!result)
                return NotFound("No se encontro la disponibilidad para la cedula indicada.");
            return NoContent();
        }
    }
}