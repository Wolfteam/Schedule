using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Schedule.API.Models;
using Schedule.API.Models.Repositories;
using Schedule.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Schedule.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class AccountController : BaseController
    {
        public AccountController(IUnitOfWork uow, IMapper mapper)
            : base(uow, mapper)
        {
        }

        // POST api/Account
        [Authorize(Roles = Roles.ADMINISTRADOR)]
        [HttpPost]
        public IActionResult Add([FromBody] UsuarioDTO usuario)
        {
            bool userAlreadyExist = UserExists(usuario.Cedula);
            if (userAlreadyExist)
                return BadRequest("El usuario ya existe");
            _db.UsuarioRepository.Add(_mapper.Map<Admin>(usuario));
            bool result = _db.Save();
            if (!result)
                return StatusCode(500, "Ocurrio un error al crear el usuario");
            return CreatedAtRoute("GetUsuario", new { cedula = usuario.Cedula }, usuario);
        }

        // GET api/Account
        [Authorize(Roles = Roles.ADMINISTRADOR)]
        [HttpGet]
        public IEnumerable<UsuarioDetailsDTO> GetAll()
        {
            var usuarios = _db.UsuarioRepository.Get(includeProperties: "CedulaNavigation, IdPrivilegioNavigation");
            return _mapper.Map<IEnumerable<UsuarioDetailsDTO>>(usuarios);
        }

        // GET api/Account/21255727
        [Authorize(Roles = Roles.ADMINISTRADOR)]
        [HttpGet("{cedula}", Name = "GetUsuario")]
        public IActionResult Get(uint cedula)
        {
            //var usuario = _db.UsuarioRepository.Get(u => u.Cedula == cedula, includeProperties: "CedulaNavigation, IdPrivilegioNavigation");
            var usuario = _db.UsuarioRepository.Get(cedula);
            if (usuario == null)
                return NotFound("No se encontro el usuario buscado.");
            return new ObjectResult(_mapper.Map<UsuarioDetailsDTO>(usuario));
        }

        // DELETE api/Account/21255727
        [Authorize(Roles = Roles.ADMINISTRADOR)]
        [HttpDelete("{cedula}")]
        public IActionResult Remove(uint cedula)
        {
            _db.UsuarioRepository.Remove(cedula);
            bool result = _db.Save();
            if (!result)
                return StatusCode(500, $"Ocurrio un error al borrar el usuario con la cedula {cedula}");
            return new NoContentResult();
        }

        // DELETE api/Account?cedulas=1,2,3,4
        [Authorize(Roles = Roles.ADMINISTRADOR)]
        [HttpDelete]
        public IActionResult Delete([FromQuery] string cedulas)
        {
            uint[] usersToRemove = cedulas.Split(",").Select(value => uint.Parse(value.Trim())).ToArray();
            foreach (var usuario in usersToRemove)
                _db.UsuarioRepository.Remove(usuario);
            bool result = _db.Save();
            if (!result)
                return StatusCode(500, $"Ocurrio un error al borrar las cedulas: {cedulas}");
            return new NoContentResult();
        }

        // PUT api/Account/21255727
        [Authorize(Roles = Roles.ADMINISTRADOR)]
        [HttpPut("{cedula}")]
        public IActionResult Update(uint cedula, [FromBody] UsuarioDTO usuario)
        {
            if (!UserExists(cedula) || (cedula != usuario.Cedula && UserExists(usuario.Cedula)))
                return NotFound($"No existe el usuario {cedula} a actualizar o la nueva cedula {usuario.Cedula} no existe.");
            _db.UsuarioRepository.Update(cedula, _mapper.Map<Admin>(usuario));
            bool result = _db.Save();
            if (!result)
                return StatusCode(500, $"Ocurrio un error al actualizar el usuario {cedula}");
            return new NoContentResult();
        }

        [HttpPut("{cedula}/ChangePassword")]
        public IActionResult ChangePassword(uint cedula, [FromBody] ChangePasswordDTO request)
        {
            var resultDTO = new ResultDTO
            {
                Succeed = false
            };

            if (request.NewPassword != request.NewPasswordConfirmation)
            {
                resultDTO.Message = "Las password introducidas no concuerdan";
                return BadRequest(resultDTO);
            }

            bool userExists = UserExists(cedula);
            if (!userExists)
            {
                resultDTO.Message = $"No existe un usuario con la cedula {cedula}";
                return BadRequest(resultDTO);
            }

            bool isCurrentPasswordValid = _db.UsuarioRepository.IsCurrentPasswordValid(cedula, request.CurrentPassword);
            if (!isCurrentPasswordValid)
            {
                resultDTO.Message = "La password actual no coincide con la enviada";
                return BadRequest(resultDTO);
            }

            _db.UsuarioRepository.ChangePassword(cedula, request.NewPassword);
            resultDTO.Succeed = _db.Save();
            if (!resultDTO.Succeed)
            {
                resultDTO.Message = $"Ocurrio un error al actualizar la password del usuario {cedula}";
                return StatusCode(500, resultDTO);
            }
            resultDTO.Message = "Password actualizado exitosamente";
            return Ok(resultDTO);
        }

        private bool UserExists(uint cedula)
        {
            return _db.UsuarioRepository.Exists(u => u.Cedula == cedula);
        }
    }
}