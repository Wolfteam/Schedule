using Microsoft.AspNetCore.Mvc;
using Schedule.API.Filters;
using Schedule.API.Models;
using Schedule.API.Models.Repositories;
using Schedule.Entities;

namespace Schedule.API.Controllers
{
    [Route("api/[controller]")]
    [GlobalAttibute]
    public class DisponibilidadController : Controller
    {
        private readonly DisponibilidadProfesorRepository _db = new DisponibilidadProfesorRepository();

        // POST api/Disponibilidad/Create
        [HttpPost("Create")]
        //[AuthenticateAttribute].
        public bool Create([FromBody] DisponibilidadProfesores disponibilidad)
        {
            return _db.Create(disponibilidad);
        }

        // DELETE api/Disponibilidad/Delete/21255727
        [HttpDelete("Delete/{cedula}")]
        //[AuthenticateAttribute]
        public bool Delete(int cedula)
        {
            return _db.Delete(cedula);
        }

        // DELETE api/Disponibilidad/Delete
        [HttpDelete("Delete")]
        //[AuthenticateAttribute]
        public bool Delete()
        {
            return _db.Delete();
        }

        // GET api/Disponibilidad/Get/21255727
        [HttpGet("Get/{cedula}")]
        //[AuthenticateAttribute]
        public DisponibilidadProfesorDTO Get(int cedula)
        {
            return _db.Get(cedula);
        }
    }
}