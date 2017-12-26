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
    [Authorize(Roles = "Administrador")]
    public class UsuariosController : BaseController
    {
        private readonly UnitOfWork _unitOfWork;
        public UsuariosController(IOptions<AppSettings> appSettings, IHttpClientsFactory httpClientsFactory)
            : base(appSettings, httpClientsFactory)
        {
            _unitOfWork = new UnitOfWork(httpClientsFactory);
        }
        
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] UsuarioDTO usuario)
        {
            _unitOfWork.Token = await HttpContext.GetTokenAsync(_tokenName);
            bool result = await _unitOfWork.UsuarioRepository.AddAsync(usuario);
            if (!result)
                return StatusCode(500);
            return CreatedAtRoute("GetUsuario", new { cedula = usuario.Cedula }, usuario);
        }

        [HttpGet("{cedula}", Name = "GetUsuario")]
        public async Task<UsuarioDetailsDTO> GetAsync(int cedula)
        {
            _unitOfWork.Token = await HttpContext.GetTokenAsync(_tokenName);
            return await _unitOfWork.UsuarioRepository.GetAsync(cedula);
        }

        [HttpGet]
        public async Task<IEnumerable<UsuarioDetailsDTO>> GetAllAsync()
        {
            _unitOfWork.Token = await HttpContext.GetTokenAsync(_tokenName);
            return await _unitOfWork.UsuarioRepository.GetAllAsync();
        }

        [HttpDelete("{cedula}")]
        public async Task<IActionResult> RemoveAsync(int cedula)
        {
            _unitOfWork.Token = await HttpContext.GetTokenAsync(_tokenName);
            bool result = await _unitOfWork.UsuarioRepository.RemoveAsync(cedula);
            if (!result)
                return NotFound("No existe el usuario a borrar.");
            return NoContent();
        }

        [HttpPut("{cedula}")]
        public async Task<IActionResult> UpdateAsync(int cedula, [FromBody] UsuarioDTO usuario)
        {
            _unitOfWork.Token = await HttpContext.GetTokenAsync(_tokenName);
            bool result = await _unitOfWork.UsuarioRepository.UpdateAsync(cedula, usuario);
            if (!result)
                return NotFound("No existe el usuario a actualizar.");
            return NoContent();
        }
    }
}