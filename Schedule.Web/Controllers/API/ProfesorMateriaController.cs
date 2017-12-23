using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Schedule.Entities;
using Schedule.Web.Filters;
using Schedule.Web.Models;
using Schedule.Web.Models.Repository;

namespace Schedule.Web.Controllers.API
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrador")]
    public class ProfesorMateriaController : BaseController
    {
        private UnitOfWork _unitOfWork;

        public ProfesorMateriaController(IOptions<AppSettings> appSettings, IHttpClientsFactory httpClientsFactory)
            : base(appSettings, httpClientsFactory)
        {
            _unitOfWork = new UnitOfWork(_httpClientsFactory.GetClient(_apiHttpClientName));

        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] ProfesorMateriaDTO pm)
        {
            bool result = await _unitOfWork.ProfesorMateriaRepository.AddAsync(pm);
            if (!result)
                return StatusCode(500);
            //return CreatedAtRoute("GetProfesorMateria", new { id = relacion.Id }, pm);
            return NoContent();
        }

        [HttpGet("{id}", Name = "GetProfesorMateria")]
        public async Task<ProfesorMateriaDetailsDTO> GetAsync(int id)
        {
            return await _unitOfWork.ProfesorMateriaRepository.GetAsync(id);
        }

        [HttpGet]
        public async Task<IEnumerable<ProfesorMateriaDetailsDTO>> GetAllAsync()
        {
            return await _unitOfWork.ProfesorMateriaRepository.GetAllAsync();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveAsync(int id)
        {
            bool result = await _unitOfWork.ProfesorMateriaRepository.RemoveAsync(id);
            if (!result)
                return NotFound("No se encontro la relacion a borrar.");
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] ProfesorMateriaDTO pm)
        {
            bool result = await _unitOfWork.ProfesorMateriaRepository.UpdateAsync(id, pm);
            if (!result)
                return NotFound("No se encontro la relaciona a actualizar.");
            return NoContent();
        }
    }
}