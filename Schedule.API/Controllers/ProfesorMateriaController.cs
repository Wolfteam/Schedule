using Microsoft.AspNetCore.Mvc;
using Schedule.API.Filters;
using Schedule.BLL;
using Schedule.Entities;
using System.Collections.Generic;

namespace Schedule.API.Controllers
{
    [Route("api/[controller]")]
    [GlobalAttibute]
    public class ProfesorMateriaController : Controller
    {
        // POST api/ProfesorMateria/Create
        [HttpPost("Create")]
        //[AuthenticateAttribute].
        //[AuthorizationAttribute(Privilegios.Administrador)]
        public bool Create([FromBody] ProfesorMateria pm)
        {
            return new ProfesorMateriaBLL().Create(pm);
        }

        // DELETE api/ProfesorMateria/Delete/10
        [HttpDelete("Delete/{id}")]
        //[AuthenticateAttribute]
        //[AuthorizationAttribute(Privilegios.Administrador)]
        public bool Delete(int id)
        {
            //TODO: Agregar un campo autoincrement a la tabla
            return new ProfesorMateriaBLL().Delete(id);
        }

        // GET api/ProfesorMateria/GetAll
        [HttpGet("GetAll")]
        //[AuthenticateAttribute]
        public List<ProfesorMateria> GetAll()
        {
            return new ProfesorMateriaBLL().GetAll();
        }
    }
}