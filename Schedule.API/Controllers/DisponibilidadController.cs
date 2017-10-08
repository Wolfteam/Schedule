using Microsoft.AspNetCore.Mvc;
using Schedule.API.Filters;
using Schedule.BLL;
using Schedule.Entities;
using System.Collections.Generic;

namespace Schedule.API.Controllers
{
    [Route("api/[controller]")]
    [GlobalAttibute]
    public class DisponibilidadController : Controller
    {
        // POST api/Disponibilidad/Create
        [HttpPost("Create")]
        //[AuthenticateAttribute].
        public bool Create([FromBody] DisponibilidadProfesor disponibilidad)
        {
            return new DisponibilidadProfesorBLL().Create(disponibilidad);
        }

        // DELETE api/Disponibilidad/Delete/21255727
        [HttpDelete("Delete/{cedula}")]
        //[AuthenticateAttribute]
        public bool Delete(int cedula)
        {
            return new DisponibilidadProfesorBLL().Delete(cedula);
        }

        // DELETE api/Disponibilidad/Delete
        [HttpDelete("Delete")]
        //[AuthenticateAttribute]
        public bool Delete()
        {
            return new DisponibilidadProfesorBLL().Delete();
        }

        // GET api/Disponibilidad/Get/21255727
        [HttpGet("Get/{cedula}")]
        //[AuthenticateAttribute]
        public DisponibilidadProfesor Get(int cedula)
        {
            return new DisponibilidadProfesorBLL().Get(cedula);
        }
    }
}