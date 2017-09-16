using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Schedule.BLL;
using Schedule.Entities;

namespace Schedule.API.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        // GET: api/Account
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "Account1", "Account2" };
        }

        // GET api/Account/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/Account/Login
        [Route("Login")]
        [HttpPost]
        public bool Login([FromBody]Usuario usuario)
        {
            return new UsuarioBLL().AuthenticateUser(usuario.Username, usuario.Password);
        }

        //api/Account/Test
        [Route("Test")]
        [HttpGet]
        public string Test()
        {
            return "esto es una prueba";
        }

        // PUT api/Account/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/Account/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
