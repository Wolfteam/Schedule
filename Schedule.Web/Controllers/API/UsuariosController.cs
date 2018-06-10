using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Schedule.Entities;
using Schedule.Web.Models;
using Schedule.Web.Models.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;

namespace Schedule.Web.Controllers.API
{
    [Route("api/[controller]")]
    [Authorize]
    public class UsuariosController : BaseController
    {
        private readonly UnitOfWork _unitOfWork;
        public UsuariosController(IOptions<AppSettings> appSettings, IHttpClientsFactory httpClientsFactory)
            : base(appSettings, httpClientsFactory)
        {
            _unitOfWork = new UnitOfWork(httpClientsFactory);
        }

        [HttpPost]
        [Authorize(Roles = Roles.ADMINISTRADOR)]
        public async Task<IActionResult> AddAsync([FromBody] UsuarioDTO usuario)
        {
            _unitOfWork.Token = await HttpContext.GetTokenAsync(_tokenName);
            bool result = await _unitOfWork.UsuarioRepository.AddAsync(usuario);
            if (!result)
                return StatusCode(500);
            return CreatedAtRoute("GetUsuario", new { cedula = usuario.Cedula }, usuario);
        }

        [HttpGet("{cedula}", Name = "GetUsuario")]
        [Authorize(Roles = Roles.ADMINISTRADOR)]
        public async Task<UsuarioDetailsDTO> GetAsync(int cedula)
        {
            _unitOfWork.Token = await HttpContext.GetTokenAsync(_tokenName);
            return await _unitOfWork.UsuarioRepository.GetAsync(cedula);
        }

        [HttpGet]
        [Authorize(Roles = Roles.ADMINISTRADOR)]
        public async Task<IEnumerable<UsuarioDetailsDTO>> GetAllAsync()
        {
            _unitOfWork.Token = await HttpContext.GetTokenAsync(_tokenName);
            return await _unitOfWork.UsuarioRepository.GetAllAsync();
        }

        [HttpDelete("{cedula}")]
        [Authorize(Roles = Roles.ADMINISTRADOR)]
        public async Task<IActionResult> RemoveAsync(int cedula)
        {
            _unitOfWork.Token = await HttpContext.GetTokenAsync(_tokenName);
            bool result = await _unitOfWork.UsuarioRepository.RemoveAsync(cedula);
            if (!result)
                return NotFound("No existe el usuario a borrar.");
            return NoContent();
        }

        [HttpPut("{cedula}")]
        [Authorize(Roles = Roles.ADMINISTRADOR)]
        public async Task<IActionResult> UpdateAsync(int cedula, [FromBody] UsuarioDTO usuario)
        {
            _unitOfWork.Token = await HttpContext.GetTokenAsync(_tokenName);
            bool result = await _unitOfWork.UsuarioRepository.UpdateAsync(cedula, usuario);
            if (!result)
                return NotFound("No existe el usuario a actualizar.");
            return NoContent();
        }

        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordDTO request)
        {
            uint cedula = uint.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            _unitOfWork.Token = await HttpContext.GetTokenAsync(_tokenName);
            var result = await _unitOfWork.UsuarioRepository.ChangePassword(cedula, request);
            return Ok(result);
        }
    }
}