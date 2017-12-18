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
        private readonly UnitOfWork _db = new UnitOfWork();

        // POST api/ProfesorMateria
        [HttpPost]
        public IActionResult Create([FromBody] ProfesorMateriaDTO pm)
        {
            _db.ProfesorMateriaRepository.Add(Mapper.Map<ProfesorMateriaDTO, ProfesoresMaterias>(pm));
            bool result = _db.Save();
            if (!result)
                return StatusCode(500);
            var relacion = _db.ProfesorMateriaRepository.Get(pm.Cedula, pm.Codigo);
            return CreatedAtRoute("GetProfesorMateria", new { id = relacion.Id }, pm);
        }

        // DELETE api/ProfesorMateria/10
        [HttpDelete("{id}")]
        public IActionResult Delete(uint id)
        {
            _db.ProfesorMateriaRepository.Remove(id);
            bool result = _db.Save();
            if (!result)
                return NotFound("No se encontro la relacion a borrar.");
            return new NoContentResult();
        }

        // GET api/ProfesorMateria
        [HttpGet]
        public IEnumerable<ProfesorMateriaDetailsDTO> GetAll()
        {
            var relaciones = _db.ProfesorMateriaRepository.GetAll();
            return Mapper.Map<IEnumerable<ProfesorMateriaDetailsDTO>>(relaciones);
        }

        // GET api/ProfesorMateria/1234
        [HttpGet("{id}", Name = "GetProfesorMateria")]
        public ProfesorMateriaDetailsDTO Get(uint id)
        {
            var relacion = _db.ProfesorMateriaRepository.Get(id);
            return Mapper.Map<ProfesoresMaterias, ProfesorMateriaDetailsDTO>(relacion);
        }

        // PUT api/ProfesorMateria/4
        [HttpPut("{id}")]
        public IActionResult Update(uint id, [FromBody] ProfesorMateriaDTO pm)
        {
            _db.ProfesorMateriaRepository.Update(id, Mapper.Map<ProfesorMateriaDTO, ProfesoresMaterias>(pm));
            bool result = _db.Save();
            if (!result)
                return NotFound("No se encontro la relaciona a actualizar.");
            return new NoContentResult();
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}