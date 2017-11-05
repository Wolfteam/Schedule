using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Schedule.API.Filters;
using Schedule.API.Models;
using Schedule.API.Models.Repositories;
using Schedule.Entities;
using System.Collections.Generic;

namespace Schedule.API.Controllers
{
    [Route("api/[controller]")]
    [GlobalAttibute]
    public class ProfesorController : Controller
    {
        private readonly ProfesorRepository _db = new ProfesorRepository();

        // POST api/Profesor
        [HttpPost]
        //[AuthenticateAttribute].
        //[AuthorizationAttribute(Privilegios.Administrador)]
        public IActionResult Create([FromBody] ProfesorDTO profesor)
        {
            bool result = _db.Create(Mapper.Map<ProfesorDTO, Profesores>(profesor));
            if (!result)
                return StatusCode(500);
            return CreatedAtRoute("GetProfesor", new { cedula = profesor.Cedula }, profesor);
        }

        // DELETE api/Profesor/21255727
        [HttpDelete("{cedula}")]
        //[AuthenticateAttribute]
        //[AuthorizationAttribute(Privilegios.Administrador)]
        public IActionResult Delete(int cedula)
        {
            bool result = _db.Delete(cedula);
            if (!result)
                return StatusCode(404);
            return new NoContentResult();
        }

        // GET api/Profesor
        [HttpGet]
        //[AuthenticateAttribute]
        public IEnumerable<ProfesorDetailsDTO> GetAll()
        {
            return _db.Get();
        }

        // GET api/Profesor/1
        [HttpGet("{cedula}", Name = "GetProfesor")]
        //[AuthenticateAttribute]
        public IActionResult Get(int cedula)
        {
            var profesor = _db.Get(cedula);
            if (profesor == null)
                return NotFound();
            return new ObjectResult(profesor);
        }

        // PUT api/Profesor/21255727
        [HttpPut("{cedula}")]
        public IActionResult Update(int cedula, [FromBody] ProfesorDTO profesor)
        {
            bool result = _db.Update(cedula, Mapper.Map<ProfesorDTO, Profesores>(profesor));
            if (!result)
                return StatusCode(404);
            return new NoContentResult();
        }
    }
}