using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Schedule.API.Filters;
using Schedule.API.Models;
using Schedule.API.Models.Repositories;
using Schedule.Entities;
using System.Collections.Generic;
using System.Linq;

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
        public IActionResult Create([FromBody] IEnumerable<DisponibilidadProfesorDTO> disponibilidad)
        {
            bool result = _db.Create(Mapper.Map<IEnumerable<DisponibilidadProfesorDTO>, IEnumerable<DisponibilidadProfesores>>(disponibilidad));
            if (!result)
                return StatusCode(500);
            return CreatedAtRoute("GetDisponibilidad", new { cedula = disponibilidad.FirstOrDefault().Cedula }, disponibilidad);
        }

        // DELETE api/Disponibilidad/21255727
        [HttpDelete("{cedula}")]
        //[AuthenticateAttribute]
        public IActionResult Delete(uint cedula)
        {
            bool result = _db.Delete(cedula);
            if (!result)
                return NotFound();
            return new NoContentResult();
        }

        // DELETE api/Disponibilidad
        [HttpDelete]
        //[AuthenticateAttribute]
        public IActionResult Delete()
        {
            bool result = _db.Delete();
            if (!result)
                return StatusCode(500);
            return new NoContentResult();
        }

        // GET api/Disponibilidad/21255727
        [HttpGet("{cedula}", Name = "GetDisponibilidad")]
        //[AuthenticateAttribute]
        public IActionResult Get(int cedula)
        {
            //TODO: Aca deberia ir a pedir las horas a cumplir del profesor en el repo del mismo
            //seria weno aplicar el repo pattern de una vez pa no tener q crear una variable
            //para cada repo
            var disponibilidad = _db.Get(cedula);
            if (disponibilidad.Disponibilidad != null)
                return new ObjectResult(disponibilidad);
            ProfesorRepository pr = new ProfesorRepository();
            disponibilidad.Cedula = (uint)cedula;
            //asumo que la cedula existe
            disponibilidad.HorasACumplir = pr.GetHorasACumplir(cedula);
            return new ObjectResult(disponibilidad);
        }
    }
}