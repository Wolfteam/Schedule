using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
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
            _unitOfWork = new UnitOfWork(httpClientsFactory);
        }

        [Authorize(Roles = Roles.ADMINISTRADOR)]
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] PeriodoCarreraDTO periodo)
        {
            _unitOfWork.Token = await HttpContext.GetTokenAsync(_tokenName);
            bool result = await _unitOfWork.PeriodoCarreraRepository.AddAsync(periodo);
            if (!result)
                return StatusCode(500);
            return CreatedAtRoute("GetPeriodoCarrera", new { id = 0 }, periodo);
        }

        [Authorize(Roles = Roles.ADMINISTRADOR)]
        [HttpGet("{id}", Name = "GetPeriodoCarrera")]
        public async Task<PeriodoCarreraDTO> GetAsync(int id)
        {
            _unitOfWork.Token = await HttpContext.GetTokenAsync(_tokenName);
            return await _unitOfWork.PeriodoCarreraRepository.GetAsync(id);
        }

        [Authorize(Roles = Roles.ADMINISTRADOR)]
        [HttpGet]
        public async Task<IEnumerable<PeriodoCarreraDTO>> GetAllAsync()
        {
            _unitOfWork.Token = await HttpContext.GetTokenAsync(_tokenName);
            return await _unitOfWork.PeriodoCarreraRepository.GetAllAsync();
        }

        [HttpGet("Current")]
        public async Task<PeriodoCarreraDTO> GetCurrentPeriodoAsync()
        {
            _unitOfWork.Token = await HttpContext.GetTokenAsync(_tokenName);
            return await _unitOfWork.PeriodoCarreraRepository.GetCurrentPeriodoAsync();
        }

        [Authorize(Roles = Roles.ADMINISTRADOR)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveAsync(int id)
        {
            _unitOfWork.Token = await HttpContext.GetTokenAsync(_tokenName);
            bool result = await _unitOfWork.PeriodoCarreraRepository.RemoveAsync(id);
            if (!result)
                return NotFound("No existe el periodo academico a borrar.");
            return NoContent();
        }

        [Authorize(Roles = Roles.ADMINISTRADOR)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] PeriodoCarreraDTO periodo)
        {
            _unitOfWork.Token = await HttpContext.GetTokenAsync(_tokenName);
            bool result = await _unitOfWork.PeriodoCarreraRepository.UpdateAsync(id, periodo);
            if (!result)
                return NotFound("No existe el periodo academico a actualizar.");
            return NoContent();
        }
    }
}