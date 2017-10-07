using Microsoft.AspNetCore.Mvc;
using Schedule.BLL;
using Schedule.Entities;
using System.Collections.Generic;

namespace Schedule.API.Controllers
{
    [Route("api/[controller]")]
    public class SeccionesController : Controller
    {
        // POST api/Secciones/Create
        [HttpPost("Create")]
        //[AuthenticateAttribute].
        //[AuthorizationAttribute(Privilegios.Administrador)]
        public bool Create([FromBody] Secciones seccion)
        {
            return new SeccionesBLL().Create(seccion);
        }

        // DELETE api/Secciones/Delete/44605
        [HttpDelete("Delete/{codigo}")]
        //[AuthenticateAttribute]
        //[AuthorizationAttribute(Privilegios.Administrador)]
        public bool Delete(int codigo)
        {
            return new SeccionesBLL().Delete(codigo);
        }

        // GET api/Secciones/GetAll
        [HttpGet("GetAll")]
        //[AuthenticateAttribute]
        public List<Secciones> GetAll()
        {
            return new SeccionesBLL().GetAll();
        }

        // GET api/Secciones/Get/44056
        [HttpGet("Get/{codigo}")]
        //[AuthenticateAttribute]
        public Secciones Get(int codigo)
        {
            return new SeccionesBLL().Get(codigo);
        }

        // PUT api/Secciones/Update/44056
        [HttpPut("Update/{codigo}")]
        public bool Update(int codigo, [FromBody] Secciones seccion)
        {
            return new SeccionesBLL().Update(codigo, seccion);
        }
    }
}
