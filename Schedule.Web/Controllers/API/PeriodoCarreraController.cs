using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Schedule.Entities;
using Schedule.Web.Controllers;
using Schedule.Web.Models;
using Schedule.Web.Models.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Schedule.Web.Controllers.API
{
    [Route("api/[controller]")]
    [Authorize]
    public class PeriodoCarreraController : BaseController
    {
        private readonly UnitOfWork _unitOfWork;
        public PeriodoCarreraController(IOptions<AppSettings> appSettings, IHttpClientsFactory httpClientsFactory)
            : base(appSettings, httpClientsFactory)
        {
            _unitOfWork = new UnitOfWork(_httpClientsFactory.GetClient(_apiHttpClientName));
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] PeriodoCarreraDTO periodo)
        {
            bool result = await _unitOfWork.PeriodoCarreraRepository.AddAsync(periodo);
            if (!result)
                return StatusCode(500);
            return CreatedAtRoute("GetPeriodoCarrera", new { id = 0 }, periodo);
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet("{id}", Name = "GetPeriodoCarrera")]
        public async Task<PeriodoCarreraDTO> GetAsync(int id)
        {
            return await _unitOfWork.PeriodoCarreraRepository.GetAsync(id);
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<IEnumerable<PeriodoCarreraDTO>> GetAllAsync()
        {
            return await _unitOfWork.PeriodoCarreraRepository.GetAllAsync();
        }

        [HttpGet("Current")]
        public async Task<PeriodoCarreraDTO> GetCurrentPeriodoAsync()
        {
            return await _unitOfWork.PeriodoCarreraRepository.GetCurrentPeriodoAsync();
        }

        [Authorize(Roles = "Administrador")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveAsync(int id)
        {
            bool result = await _unitOfWork.PeriodoCarreraRepository.RemoveAsync(id);
            if (!result)
                return NotFound("No existe el periodo academico a borrar.");
            return NoContent();
        }

        [Authorize(Roles = "Administrador")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] PeriodoCarreraDTO periodo)
        {
            bool result = await _unitOfWork.PeriodoCarreraRepository.UpdateAsync(id, periodo);
            if (!result)
                return NotFound("No existe el periodo academico a actualizar.");
            return NoContent();
        }
    }
}