using Microsoft.AspNetCore.Mvc;
using Schedule.API.Filters;
using Schedule.API.Models;
using Schedule.API.Models.Repositories;

namespace Schedule.API.Controllers
{
    [Route("api/[controller]")]
    [GlobalAttibute]
    public class DisponibilidadController : Controller
    {
        private readonly DisponibilidadProfesorRepository _db = new DisponibilidadProfesorRepository();

        // POST api/Disponibilidad
        [HttpPost]
        //[AuthenticateAttribute].
        public IActionResult Create([FromBody] DisponibilidadProfesores disponibilidad)
        {
            bool result = _db.Create(disponibilidad);
            if (!result)
                return StatusCode(500);
            return CreatedAtRoute("GetDisponibilidad", new { cedula = disponibilidad.Cedula }, disponibilidad);
        }

        // DELETE api/Disponibilidad/21255727
        [HttpDelete("{cedula}")]
        //[AuthenticateAttribute]
        public IActionResult Delete(int cedula)
        {
            bool result = _db.Delete(cedula);
            if (!result)
                return StatusCode(404);
            return new NoContentResult();
        }

        // DELETE api/Disponibilidad
        [HttpDelete]
        //[AuthenticateAttribute]
        public IActionResult Delete()
        {
            bool result = _db.Delete();
            if (!result)
                return StatusCode(404);
            return new NoContentResult();
        }

        // GET api/Disponibilidad/21255727
        [HttpGet("{cedula}", Name = "GetDisponibilidad")]
        //[AuthenticateAttribute]
        public IActionResult Get(int cedula)
        {
            var disponibilidad = _db.Get(cedula);
            if (disponibilidad == null)         
                return NotFound();      
            return new ObjectResult(disponibilidad);
        }
    }
}