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
    public class ProfesoresController : BaseController
    {
        private readonly UnitOfWork _unitOfWork;
        public ProfesoresController(IOptions<AppSettings> appSettings, IHttpClientsFactory httpClientsFactory)
            : base(appSettings, httpClientsFactory)
        {
            _unitOfWork = new UnitOfWork(_httpClientsFactory.GetClient(_apiHttpClientName));
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ProfesorDTO profesor)
        {
            bool result = await _unitOfWork.ProfesorRepository.AddAsync(profesor);
            if (result)
                return CreatedAtRoute("GetProfesor", new { cedula = profesor.Cedula }, profesor);
            else
                return NotFound("No se encontro el profesor a borrar.");
        }

        [HttpGet]
        public async Task<IEnumerable<ProfesorDetailsDTO>> GetAll()
        {
            return await _unitOfWork.ProfesorRepository.GetAllAsync();
        }

        [HttpGet("{cedula}", Name = "GetProfesor")]
        public async Task<ProfesorDetailsDTO> Get(int cedula)
        {
            return await _unitOfWork.ProfesorRepository.GetAsync(cedula);
        }

        [HttpDelete("{cedula}")]
        public async Task<IActionResult> Remove(int cedula)
        {
            bool result = await _unitOfWork.ProfesorRepository.RemoveAsync(cedula);
            if (result)
                return NoContent();
            else
                return NotFound("No se encontro el profesor a borrar.");
        }

        [HttpPut("{cedula}")]
        public async Task<IActionResult> Update(int cedula, [FromBody] ProfesorDTO profesor)
        {
            bool result = await _unitOfWork.ProfesorRepository.UpdateAsync(cedula, profesor);
            if (result)
                return NoContent();
            else
                return NotFound("No se encontro el profesor a actualizar.");
        }
    }
}