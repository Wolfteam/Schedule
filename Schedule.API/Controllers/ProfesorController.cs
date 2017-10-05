using Microsoft.AspNetCore.Mvc;
using Schedule.API.Filters;
using Schedule.BLL;
using Schedule.Entities;
using System.Collections.Generic;

namespace Schedule.API.Controllers
{
    [Route("api/[controller]")]
    [GlobalAttibute]
    public class ProfesorController : Controller
    {
        // POST api/Profesor/Create
        [HttpPost("Create")]
        //[AuthenticateAttribute].
        //[AuthorizationAttribute(Privilegios.Administrador)]
        public bool Create([FromBody] Profesor profesor)
        {
            return new ProfesorBLL().Create(profesor);
        }

        // DELETE api/Profesor/Delete/21255727
        [HttpDelete("Delete/{cedula}")]
        //[AuthenticateAttribute]
        //[AuthorizationAttribute(Privilegios.Administrador)]
        public bool Delete(int cedula)
        {
            return new ProfesorBLL().Delete(cedula);
        }

        // GET api/Profesor/GetAll
        [HttpGet("GetAll")]
        //[AuthenticateAttribute]
        public List<Profesor> GetAll()
        {
            return new ProfesorBLL().GetAll();
        }

        // GET api/Profesor/Get/1
        [HttpGet("Get/{cedula}")]
        //[AuthenticateAttribute]
        public Profesor Get(int cedula)
        {
            return new ProfesorBLL().Get(cedula);
        }

        // PUT api/Profesor/Update/21255727
        [HttpPut("Update/{cedula}")]
        public bool Update(int cedula, [FromBody] Profesor profesor)
        {
            return new ProfesorBLL().Update(cedula, profesor);
        }
    }
}
