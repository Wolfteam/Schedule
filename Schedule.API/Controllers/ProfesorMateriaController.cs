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
    [Authorize(Roles = Roles.ADMINISTRADOR)]
    public class ProfesorMateriaController : BaseController
    {
        public ProfesorMateriaController(HorariosContext context)
            : base(context)
        {
        }

        // POST api/ProfesorMateria
        [HttpPost]
        public IActionResult Create([FromBody] ProfesorMateriaDTO pm)
        {
            _db.ProfesorMateriaRepository.Add(Mapper.Map<ProfesorMateriaDTO, ProfesoresMaterias>(pm));
            bool result = _db.Save();
            if (!result)
                return StatusCode(500, $"Ocurrio un error al crear la relacion {pm.Cedula} - {pm.Codigo}");
            var relacion = _db.ProfesorMateriaRepository.Get(pm.Cedula, pm.Codigo);
            return CreatedAtRoute("GetProfesorMateria", new { id = relacion.Id }, pm);
        }

        // DELETE api/ProfesorMateria/10
        [HttpDelete("{id}")]
        public IActionResult Delete(uint id)
        {
            var entity = _db.ProfesorMateriaRepository.Get(pm => pm.Id == id).FirstOrDefault();
            _db.ProfesorMateriaRepository.Remove(entity);
            bool result = _db.Save();
            if (!result)
                return StatusCode(500, $"Ocurrio un error al borrar la relacion {id}");
            return new NoContentResult();
        }

        // DELETE api/ProfesorMateria?ids=1,2,3,4
        [HttpDelete]
        public IActionResult Delete([FromQuery] string ids)
        {
            uint[] relacionesToRemove = ids.Split(",").Select(value => uint.Parse(value.Trim())).ToArray();
            foreach (var relacion in relacionesToRemove)
                _db.ProfesorMateriaRepository.Remove(relacion);
            bool result = _db.Save();
            if (!result)
                return StatusCode(500, $"Ocurrio un error al borrar la relacion {ids}");
            return new NoContentResult();
        }

        // GET api/ProfesorMateria
        [HttpGet]
        public IEnumerable<ProfesorMateriaDetailsDTO> GetAll()
        {
            var relaciones = _db.ProfesorMateriaRepository.Get(includeProperties: "Profesores, Materias, Materias.Semestres, Materias.Carreras, Materias.TipoAulaMaterias");
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

            var oldEntity = _db.ProfesorMateriaRepository.Get(p => p.Id == id).FirstOrDefault();
            if (oldEntity == null)
                return NotFound("La relacion a actualizar no existe");
            _db.ProfesorMateriaRepository.Remove(oldEntity);
            _db.ProfesorMateriaRepository.Add(Mapper.Map<ProfesorMateriaDTO, ProfesoresMaterias>(pm));
            bool result = _db.Save();
            if (!result)
                return StatusCode(500, $"Ocurrio un error al intentar actualizar la relacion {id} : {pm.Cedula} - {pm.Codigo}.");
            return new NoContentResult();
        }
    }
}