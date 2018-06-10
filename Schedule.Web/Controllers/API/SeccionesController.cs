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

namespace Schedule.Web.Controllers.API
{
    [Route("api/[controller]")]
    [Authorize(Roles = Roles.ADMINISTRADOR)]
    public class SeccionesController : BaseController
    {
        private UnitOfWork _unitOfWork;
        public SeccionesController(IOptions<AppSettings> appSettings, IHttpClientsFactory httpClientsFactory)
            : base(appSettings, httpClientsFactory)
        {
            _unitOfWork = new UnitOfWork(httpClientsFactory);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] SeccionesDTO seccion)
        {
            _unitOfWork.Token = await HttpContext.GetTokenAsync(_tokenName);
            bool result = await _unitOfWork.SeccionesRepository.AddAsync(seccion);
            if (!result)
                return StatusCode(500);
            return CreatedAtRoute("GetSeccion", new { codigo = seccion.Codigo }, seccion);
        }

        [HttpGet("{codigo}", Name = "GetSeccion")]
        public async Task<SeccionesDetailsDTO> GetAsync(int codigo)
        {
            _unitOfWork.Token = await HttpContext.GetTokenAsync(_tokenName);
            return await _unitOfWork.SeccionesRepository.GetAsync(codigo);
        }

        [HttpGet]
        public async Task<IEnumerable<SeccionesDetailsDTO>> GetAllAsync()
        {
            _unitOfWork.Token = await HttpContext.GetTokenAsync(_tokenName);
            return await _unitOfWork.SeccionesRepository.GetAllAsync();
        }

        [HttpDelete("{codigo}")]
        public async Task<IActionResult> RemoveAsync(int codigo)
        {
            _unitOfWork.Token = await HttpContext.GetTokenAsync(_tokenName);
            bool result = await _unitOfWork.SeccionesRepository.RemoveAsync(codigo);
            if (!result)
                return NotFound("No se encontro la seccion a borrar.");
            return NoContent();
        }

        [HttpPut("{codigo}")]
        public async Task<IActionResult> UpdateAsync(int codigo, [FromBody] SeccionesDTO aula)
        {
            _unitOfWork.Token = await HttpContext.GetTokenAsync(_tokenName);
            bool result = await _unitOfWork.SeccionesRepository.UpdateAsync(codigo, aula);
            if (!result)
                return NotFound("No se encontro la seccion a actualizar.");
            return NoContent();
        }
    }
}