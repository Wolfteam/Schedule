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

        // DELETE api/ProfesorMateria/Delete/21255727
        [HttpDelete("Delete/{cedula}")]
        //[AuthenticateAttribute]
        //[AuthorizationAttribute(Privilegios.Administrador)]
        public bool Delete(int cedula)
        {
            //TODO: PENSAR ESTO MEJOR
            //return new ProfesorMateriaBLL().Delete(cedula);
        }


    }
}