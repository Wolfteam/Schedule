using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Schedule.Entities;
using Schedule.Web.Controllers;
using Schedule.Web.Models;
using Schedule.Web.Models.Repository;
using System.Threading.Tasks;

namespace Schedule.Web.Controllers.API
{
    [Route("api/[controller]")]
    [Authorize]
    public class HorarioProfesorController : BaseController
    {
        private readonly UnitOfWork _unitOfWork;
        public HorarioProfesorController(IOptions<AppSettings> appSettings, IHttpClientsFactory httpClientsFactory)
            : base(appSettings, httpClientsFactory)
        {
            _unitOfWork = new UnitOfWork(httpClientsFactory);
        }

        [HttpGet("PlanificacionAcademica")]
        public async Task<IActionResult> GeneratePlanificacionAcademica()
        {
            _unitOfWork.Token = await HttpContext.GetTokenAsync(_tokenName);
            return await _unitOfWork.HorarioProfesorRepository.GeneratePlanificacionAcademica();
        }


        [HttpGet("PlanificacionAulas")]
        public async Task<IActionResult> GeneratePlanificacionAulas()
        {
            _unitOfWork.Token = await HttpContext.GetTokenAsync(_tokenName);
            return await _unitOfWork.HorarioProfesorRepository.GeneratePlanificacionAulas();
        }

        [HttpGet("PlanificacionHorario")]
        public async Task<IActionResult> GeneratePlanificacionHorario()
        {
            _unitOfWork.Token = await HttpContext.GetTokenAsync(_tokenName);
            return await _unitOfWork.HorarioProfesorRepository.GeneratePlanificacionHorario();
        }
    }
}