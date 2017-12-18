﻿using Microsoft.AspNetCore.Mvc;
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
        private readonly UnitOfWork _db = new UnitOfWork();

        // POST api/Account/Login
        [HttpPost("Login")]
        public ActionResult Login([FromBody] UsuarioDTO usuario)
        {
            var periodo = _db.PeriodoCarreraRepository.GetCurrentPeriodo();
            bool userExists = _db.UsuarioRepository.UserExists(usuario.Username, usuario.Password);
            if (userExists && periodo != null)
            {
                var token = _db.TokenRepository.GenerateToken(usuario.Username);
                _db.TokenRepository.Add(token);
                bool result = _db.Save();
                if (result)
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
            _db.TokenRepository.RemoveByToken(token);
            bool result = _db.Save();
            if (result)
                return Ok();
            return NotFound($"El token {token} no fue encontrado");
        }

        //GET api/Account/Privilegios/123456798
        [HttpGet("Privilegios/{token}")]
        [AuthenticateAttribute]
        public List<Entities.Privilegios> GetAllPrivilegiosByToken(string token)
        {
            List<Entities.Privilegios> listaPrivilegios = new List<Entities.Privilegios>
            {
                _db.TokenRepository.GetAllPrivilegiosByToken(token)
            };
            return listaPrivilegios;
        }

        //GET api/Account/ProfesorInfo/123456798
        [HttpGet("ProfesorInfo/{token}")]
        [AuthenticateAttribute]
        public IActionResult GetProfesorInfoByToken(string token)
        {
            var profesor = _db.TokenRepository.GetProfesorInfoByToken(token);
            if (profesor == null)
                return NotFound("No se encontro un profesor asociado al token");
            return new ObjectResult(profesor);
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}