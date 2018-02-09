using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Schedule.API.Models;
using Schedule.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Schedule.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class ProfesorController : BaseController
    {
        public ProfesorController(HorariosContext context) 
            : base(context)
        {
        }

        // POST api/Profesor
        [Authorize(Roles = "Administrador")]
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
        [Authorize(Roles = "Administrador")]
        [HttpDelete("{cedula}")]
        public IActionResult Delete(uint cedula)
        {
            _db.ProfesorRepository.Remove(cedula);
            bool result = _db.Save();
            if (!result)
                return NotFound("No se encontro el profesor a borrar.");
            return new NoContentResult();
        }

        // DELETE api/Profesor?cedulas=1,2,3,4
        [HttpDelete]
        public IActionResult Delete([FromQuery] string cedulas)
        {
            uint[] profesoresToRemove = cedulas.Split(",").Select(value => uint.Parse(value.Trim())).ToArray();
            foreach (var profesor in profesoresToRemove)
                _db.ProfesorRepository.Remove(profesor);
            bool result = _db.Save();
            if (!result)
                return NotFound("No se encontro algun profesor a borrar.");
            return new NoContentResult();
        }

        // GET api/Profesor
        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public IEnumerable<ProfesorDetailsDTO> GetAll()
        {
            var profesores = _db.ProfesorRepository.Get(includeProperties: "PrioridadProfesor");
            return Mapper.Map<IEnumerable<ProfesorDetailsDTO>>(profesores);
        }

        // GET api/Profesor/1
        [Authorize(Roles = "Administrador, Profesor")]
        [HttpGet("{cedula}", Name = "GetProfesor")]
        public IActionResult Get(uint cedula)
        {
            var profesor = _db.ProfesorRepository.Get(cedula);
            if (profesor == null)
                return NotFound("No se encontro el profesor buscado.");
            return new ObjectResult(Mapper.Map<ProfesorDetailsDTO>(profesor));
        }

        // PUT api/Profesor/21255727
        [Authorize(Roles = "Administrador")]
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