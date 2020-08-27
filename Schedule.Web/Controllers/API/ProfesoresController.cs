using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Schedule.Entities;
using Schedule.Web.Filters;
using Schedule.Web.Models;
using Schedule.Web.Models.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Schedule.Web.Controllers.API
{
    [Route("api/[controller]")]
    [Authorize(Roles = Roles.ADMINISTRADOR)]
    public class ProfesoresController : BaseController
    {
        private readonly UnitOfWork _unitOfWork;
        public ProfesoresController(IOptions<AppSettings> appSettings, IHttpClientsFactory httpClientsFactory)
            : base(appSettings, httpClientsFactory)
        {
            _unitOfWork = new UnitOfWork(httpClientsFactory);
        }

        [HttpPost]
        public async Task<IActionResult> Add(ProfesorDTO profesor)
        {
            _unitOfWork.Token = await HttpContext.GetTokenAsync(_tokenName);
            bool result = await _unitOfWork.ProfesorRepository.AddAsync(profesor);
            if (result)
                return CreatedAtRoute("GetProfesor", new { cedula = profesor.Cedula }, profesor);
            else
                return NotFound("No se encontro el profesor a borrar.");
        }

        [HttpGet]
        public async Task<IEnumerable<ProfesorDetailsDTO>> GetAll()
        {
            _unitOfWork.Token = await HttpContext.GetTokenAsync(_tokenName);
            return await _unitOfWork.ProfesorRepository.GetAllAsync();
        }

        [HttpGet("{cedula}", Name = "GetProfesor")]
        public async Task<ProfesorDetailsDTO> Get(int cedula)
        {
            _unitOfWork.Token = await HttpContext.GetTokenAsync(_tokenName);
            return await _unitOfWork.ProfesorRepository.GetAsync(cedula);
        }

        [HttpDelete("{cedula}")]
        public async Task<IActionResult> Remove(int cedula)
        {
            _unitOfWork.Token = await HttpContext.GetTokenAsync(_tokenName);
            bool result = await _unitOfWork.ProfesorRepository.RemoveAsync(cedula);
            if (result)
                return NoContent();
            else
                return NotFound("No se encontro el profesor a borrar.");
        }

        [HttpPut("{cedula}")]
        public async Task<IActionResult> Update(int cedula, ProfesorDTO profesor)
        {
            _unitOfWork.Token = await HttpContext.GetTokenAsync(_tokenName);
            bool result = await _unitOfWork.ProfesorRepository.UpdateAsync(cedula, profesor);
            if (result)
                return NoContent();
            else
                return NotFound("No se encontro el profesor a actualizar.");
        }
    }
}