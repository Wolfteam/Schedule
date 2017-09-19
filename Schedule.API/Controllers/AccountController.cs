using Microsoft.AspNetCore.Mvc;
using Schedule.API.Filters;
using Schedule.BLL;
using Schedule.Entities;
using System.Collections.Generic;

namespace Schedule.API.Controllers
{
    [Route("api/[controller]")]
    [GlobalAttibute]
    public class AccountController : Controller
    {
        #region Variables
        private readonly ITokenService _tokenServices;
        private UsuarioBLL _usuario;
        #endregion

        #region Constructor
        public AccountController(ITokenService tokenServices)
        {
            _tokenServices = tokenServices;
            _usuario = new UsuarioBLL();
        }
        #endregion

        // POST api/Account/Login
        [HttpPost("Login")]
        public ActionResult Login([FromBody]Usuario usuario)
        {
            if (_usuario.AuthenticateUser(usuario.Username, usuario.Password))
            {
                return Ok(_tokenServices.GenerateToken(usuario.Username));
            }
            return Unauthorized();
        }

        //No ha sido implementado
        // POST api/Account/Register
        [HttpPost("Register")]
        [AuthenticateAttribute]
        [AuthorizationAttribute(Privilegios.Administrador)]
        public ActionResult Register([FromBody]Usuario usuario)
        {
            //Este metodo pide que esten autenticado y seas admin
            return Forbid();
        }

        // DELETE api/Account/123456789
        [HttpDelete("Logout/{token}")]
        [AuthenticateAttribute]
        public ActionResult Logout(string token)
        {
            if (_tokenServices.DeleteToken(token))
            {
                return Ok();
            }
            return NotFound();
        }

        //GET api/Account/123456798
        [HttpGet("Privilegios/{token}")]
        [AuthenticateAttribute]
        public List<Privilegios> GetAllPrivilegiosByToken(string token)
        {
            return _tokenServices.GetAllPrivilegiosByToken(token);
        }
    }
}


// GET: api/Account
//[HttpGet]
//public IEnumerable<string> Get()
//{
//    return new string[] { "Account1", "Account2" };
//}

//// GET api/Account/5
//[HttpGet("{id}")]
//public string Get(int id)
//{
//    return "value";
//}


//// PUT api/Account/5
//[HttpPut("{id}")]
//public void Put(int id, [FromBody]string value)
//{
//}

//// DELETE api/Account/5
//[HttpDelete("{id}")]
//public void Delete(int id)
//{
//}