using Microsoft.AspNetCore.Mvc;
using Schedule.API.Filters;
using Schedule.API.Models;
using Schedule.API.Models.Repositories;
using Schedule.Entities;
using System.Collections.Generic;

namespace Schedule.API.Controllers
{
    [Route("api/[controller]")]
    [GlobalAttibute]
    public class AccountController : Controller
    {
        private readonly UsuarioRepository _db = new UsuarioRepository();
        private readonly TokenRepository _tokenService = new TokenRepository();

        #region Variables
        //private readonly ITokenService _tokenServices;
        #endregion

        // #region Constructor
        // public AccountController(ITokenService tokenServices)
        // {
        //     _tokenServices = tokenServices;
        //     _usuario = new UsuarioBLL();
        // }
        // #endregion

        // POST api/Account/Login
        [HttpPost("Login")]
        public ActionResult Login([FromBody] UsuarioDTO usuario)
        {
            if (_db.Get(usuario.Username, usuario.Password))
            {
                var token = _tokenService.GenerateToken(usuario.Username);
                if (_tokenService.Create(token))
                {
                    return Ok(AutoMapper.Mapper.Map<Tokens, TokenDTO>(token));
                }
            }
            return Unauthorized();
        }

        // //No ha sido implementado
        // // POST api/Account/Register
        // [HttpPost("Register")]
        // [AuthenticateAttribute]
        // [AuthorizationAttribute(Privilegios.Administrador)]
        // public ActionResult Register([FromBody]Usuario usuario)
        // {
        //     //Este metodo pide que esten autenticado y seas admin
        //     return Forbid();
        // }

        // DELETE api/Account/Logout/123456789
        [HttpDelete("Logout/{token}")]
        [AuthenticateAttribute]
        public ActionResult Logout(string token)
        {
            if (_tokenService.Delete(token))
            {
                return Ok();
            }
            return NotFound();
        }

        //GET api/Account/Privilegios/123456798
        [HttpGet("Privilegios/{token}")]
        [AuthenticateAttribute]
        public List<Entities.Privilegios> GetAllPrivilegiosByToken(string token)
        {
            List<Entities.Privilegios> listaPrivilegios = new List<Entities.Privilegios>
            {
                _tokenService.GetAllPrivilegiosByToken(token)
            };
            return listaPrivilegios;
        }

        //GET api/Account/ProfesorInfo/123456798
        [HttpGet("ProfesorInfo/{token}")]
        //[AuthenticateAttribute]
        public IActionResult GetProfesorInfoByToken(string token)
        {
            var profesor = _tokenService.GetProfesorInfoByToken(token);
            if (profesor == null)
                return NotFound();
            return new ObjectResult(profesor);
        }
    }
}