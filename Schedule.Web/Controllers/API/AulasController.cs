using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "Administrador")]
    public class AulasController : BaseController
    {
        private UnitOfWork _unitOfWork;
        public AulasController(IOptions<AppSettings> appSettings, IHttpClientsFactory httpClientsFactory)
            : base(appSettings, httpClientsFactory)
        {
            _unitOfWork = new UnitOfWork(_httpClientsFactory.GetClient(_apiHttpClientName));
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] AulasDTO aula)
        {
            bool result = await _unitOfWork.AulasRepository.AddAsync(aula);
            if (!result)
                return StatusCode(500);
            return CreatedAtRoute("GetAula", new { id = aula.IdAula }, aula);
        }

        [HttpGet("{id}", Name = "GetAula")]
        public async Task<AulasDetailsDTO> GetAsync(int id)
        {
            return await _unitOfWork.AulasRepository.GetAsync(id);
        }

        [HttpGet]
        public async Task<IEnumerable<AulasDetailsDTO>> GetAllAsync()
        {
            return await _unitOfWork.AulasRepository.GetAllAsync();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveAsync(int id)
        {
            bool result = await _unitOfWork.AulasRepository.RemoveAsync(id);
            if (!result)
                return NotFound("No existe el aula a borrar.");
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] AulasDTO aula)
        {
            bool result = await _unitOfWork.AulasRepository.UpdateAsync(id, aula);
            if (!result)
                return NotFound("No existe el aula a actualizar.");
            return NoContent();
        }
    }
}