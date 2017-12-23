using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Schedule.Entities;
using Schedule.Web.Controllers;
using Schedule.Web.Models.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;
using Schedule.Web.Models;

namespace Schedule.Web.Controllers.API
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrador")]
    public class MateriasController : BaseController
    {
        private UnitOfWork _unitOfWork;
        public MateriasController(IOptions<AppSettings> appSettings, IHttpClientsFactory httpClientsFactory)
            : base(appSettings, httpClientsFactory)
        {
            _unitOfWork = new UnitOfWork(_httpClientsFactory.GetClient(_apiHttpClientName));
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] MateriasDTO materia)
        {
            bool result = await _unitOfWork.MateriasRepository.AddAsync(materia);
            if (!result)
                return StatusCode(500);
            return CreatedAtRoute("GetMateria", new { codigo = materia.Codigo }, materia);
        }

        [HttpGet("{codigo}", Name = "GetMateria")]
        public async Task<MateriasDetailsDTO> GetAsync(int codigo)
        {
            return await _unitOfWork.MateriasRepository.GetAsync(codigo);
        }

        [HttpGet]
        public async Task<IEnumerable<MateriasDetailsDTO>> GetAll()
        {
            return await _unitOfWork.MateriasRepository.GetAllAsync();
        }

        [HttpDelete("{codigo}")]
        public async Task<IActionResult> RemoveAsync(int codigo)
        {
            bool result = await _unitOfWork.MateriasRepository.RemoveAsync(codigo);
            if (!result)
                return NotFound("No se encontro la materia a borrar.");
            return NoContent();
        }
   
        [HttpPut("{codigo}")]
        public async Task<IActionResult> UpdateAsync(int codigo, [FromBody] MateriasDTO materia)
        {
            bool result = await _unitOfWork.MateriasRepository.UpdateAsync(codigo, materia);
            if (!result)
                return NotFound("No se encontro la materia a actualizar.");
            return NoContent();
        }
    }
}