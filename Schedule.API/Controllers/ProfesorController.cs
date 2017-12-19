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
    [AuthenticateAttribute]
    [AuthorizationAttribute(Entities.Privilegios.Administrador)]
    public class ProfesorController : BaseController
    {
        // POST api/Profesor
        [HttpPost]
        public IActionResult Create([FromBody] ProfesorDTO profesor)
        {
            _db.ProfesorRepository.Add(Mapper.Map<ProfesorDTO, Profesores>(profesor));
            bool result = _db.Save();
            if (!result)
                return StatusCode(500);
            return CreatedAtRoute("GetProfesor", new { cedula = profesor.Cedula }, profesor);
        }

        // DELETE api/Profesor/21255727
        [HttpDelete("{cedula}")]
        public IActionResult Delete(uint cedula)
        {
            _db.ProfesorRepository.Remove(cedula);
            bool result = _db.Save();
            if (!result)
                return NotFound("No se encontro el profesor a borrar.");
            return new NoContentResult();
        }

        // GET api/Profesor
        [HttpGet]
        public IEnumerable<ProfesorDetailsDTO> GetAll()
        {
            var profesores = _db.ProfesorRepository.Get(includeProperties:"PrioridadProfesor");
            return Mapper.Map<IEnumerable<ProfesorDetailsDTO>>(profesores);
        }

        // GET api/Profesor/1
        [HttpGet("{cedula}", Name = "GetProfesor")]
        public IActionResult Get(uint cedula)
        {
            var profesor = _db.ProfesorRepository.Get(cedula);
            if (profesor == null)
                return NotFound("No se encontro el profesor buscado.");
            return new ObjectResult(Mapper.Map<ProfesorDetailsDTO>(profesor));
        }

        // PUT api/Profesor/21255727
        [HttpPut("{cedula}")]
        public IActionResult Update(uint cedula, [FromBody] ProfesorDTO profesor)
        {
            _db.ProfesorRepository.Update(cedula, Mapper.Map<ProfesorDTO, Profesores>(profesor));
            bool result = _db.Save();
            if (!result)
                return NotFound("No se encontro el profesor a actualizar.");
            return new NoContentResult();
        }
    }
}