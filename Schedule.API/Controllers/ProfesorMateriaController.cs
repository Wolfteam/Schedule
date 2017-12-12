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
    public class ProfesorMateriaController : Controller
    {
        private readonly ProfesorMateriaRepository _db = new ProfesorMateriaRepository();

        // POST api/ProfesorMateria
        [HttpPost]
        public IActionResult Create([FromBody] ProfesorMateriaDTO pm)
        {
            bool result = _db.Create(Mapper.Map<ProfesorMateriaDTO, ProfesoresMaterias>(pm));
            if (!result)
                return StatusCode(500);
            var relacion = _db.Get(pm.Cedula, pm.Codigo);
            return CreatedAtRoute("GetProfesorMateria", new { id = relacion.Id }, pm);
        }

        // DELETE api/ProfesorMateria/10
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool result = _db.Delete(id);
            if (!result)
                return NotFound("No se encontro la relacion a borrar.");
            return new NoContentResult();
        }

        // GET api/ProfesorMateria
        [HttpGet]
        public IEnumerable<ProfesorMateriaDetailsDTO> GetAll()
        {
            return _db.Get();
        }

        // GET api/ProfesorMateria/1234
        [HttpGet("{id}", Name = "GetProfesorMateria")]
        public ProfesorMateriaDetailsDTO Get(int id)
        {
            return _db.Get(id);
        }

        // PUT api/ProfesorMateria/4
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] ProfesorMateriaDTO pm)
        {
            bool result = _db.Update(id, Mapper.Map<ProfesorMateriaDTO, ProfesoresMaterias>(pm));
            if (!result)
                return NotFound("No se encontro la relaciona a actualizar.");
            return new NoContentResult();
        }
    }
}