using Microsoft.AspNetCore.Mvc;
using Schedule.API.Filters;
using Schedule.BLL;
using Schedule.Entities;
using System.Collections.Generic;

namespace Schedule.API.Controllers
{
    [Route("api/[controller]")]
    [GlobalAttibute]
    public class AulasController : Controller
    {

        // POST api/Aulas/Create
        [HttpPost("Create")]
        //[AuthenticateAttribute].
        //[AuthorizationAttribute(Privilegios.Administrador)]
        public bool Create([FromBody] Aulas aula)
        {
            return new AulasBLL().Create(aula);
        }

        // DELETE api/Aulas/Delete/1
        [HttpDelete("Delete/{id}")]
        //[AuthenticateAttribute]
        //[AuthorizationAttribute(Privilegios.Administrador)]
        public bool Delete(int id)
        {
            return new AulasBLL().Delete(id);
        }

        // GET api/Aulas/GetAll
        [HttpGet("GetAll")]
        //[AuthenticateAttribute]
        public List<Aulas> GetAll()
        {
            return new AulasBLL().GetAll();
        }

        // GET api/Aulas/Get/1
        [HttpGet("Get/{id}")]
        //[AuthenticateAttribute]
        public Aulas Get(int id)
        {
            return new AulasBLL().Get(id);
        }
    }
}
