using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Schedule.API.Models;
using Schedule.Entities;
using System.Collections.Generic;

namespace Schedule.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrador")]
    public class AccountController : BaseController
    {
        public AccountController(HorariosContext context)
            : base(context)
        {
        }

        // POST api/Account
        [HttpPost]
        public IActionResult Add([FromBody] UsuarioDTO usuario)
        {
            _db.UsuarioRepository.Add(Mapper.Map<Admin>(usuario));
            bool result = _db.Save();
            if (!result)
                return StatusCode(500);
            return CreatedAtRoute("GetUsuario", new { cedula = usuario.Cedula }, usuario);
        }

        // GET api/Account
        [HttpGet]
        public IEnumerable<UsuarioDetailsDTO> GetAll()
        {
            var usuarios = _db.UsuarioRepository.Get(includeProperties: "CedulaNavigation, IdPrivilegioNavigation");
            return Mapper.Map<IEnumerable<UsuarioDetailsDTO>>(usuarios);
        }

        // GET api/Account/21255727
        [HttpGet("{cedula}", Name = "GetUsuario")]
        public IActionResult Get(uint cedula)
        {
            //var usuario = _db.UsuarioRepository.Get(u => u.Cedula == cedula, includeProperties: "CedulaNavigation, IdPrivilegioNavigation");
            var usuario = _db.UsuarioRepository.Get(cedula);
            if (usuario == null)
                return NotFound("No se encontro el usuario buscado.");
            return new ObjectResult(Mapper.Map<UsuarioDetailsDTO>(usuario));
        }

        // DELETE api/Account/21255727
        [HttpDelete("{cedula}")]
        public IActionResult Remove(uint cedula)
        {
            _db.UsuarioRepository.Remove(cedula);
            bool result = _db.Save();
            if (!result)
                return NotFound("No existe el usuario a borrar.");
            return new NoContentResult();
        }

        // PUT api/Account/21255727
        [HttpPut("{cedula}")]
        public IActionResult Update(uint cedula, [FromBody] UsuarioDTO usuario)
        {
            _db.UsuarioRepository.Update(cedula, Mapper.Map<Admin>(usuario));
            bool result = _db.Save();
            if (!result)
                return NotFound("No existe el usuario a actualizar");
            return new NoContentResult();
        }
    }
}