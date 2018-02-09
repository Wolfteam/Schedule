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
    [Authorize(Roles = "Administrador")]
    public class MateriasController : BaseController
    {
        public MateriasController(HorariosContext context) 
            : base(context)
        {
        }

        // POST api/Materias
        [HttpPost]
        public IActionResult Create([FromBody] MateriasDTO materia)
        {
            _db.MateriasRepository.Add(Mapper.Map<MateriasDTO, Materias>(materia));
            bool result = _db.Save();
            if (!result)
                return StatusCode(500);
            return CreatedAtRoute("GetMateria", new { codigo = materia.Codigo }, materia);
        }

        // DELETE api/Materias/34052
        [HttpDelete("{codigo}")]
        public IActionResult Delete(ushort codigo)
        {
            _db.MateriasRepository.Remove(codigo);
            bool result = _db.Save();
            if (!result)
                return NotFound("No se encontro la materia a borrar.");
            return new NoContentResult();
        }

        // DELETE api/Materias?codigos=1,2,3,4
        [HttpDelete]
        public IActionResult Delete([FromQuery] string codigos)
        {
            ushort[] materiasToRemove = codigos.Split(",").Select(value => ushort.Parse(value.Trim())).ToArray();
            foreach (var materia in materiasToRemove)
                _db.MateriasRepository.Remove(materia);
            bool result = _db.Save();
            if (!result)
                return NotFound("No se encontro alguna materia a borrar.");
            return new NoContentResult();
        }

        // GET api/Materias
        [HttpGet]
        public IEnumerable<MateriasDetailsDTO> GetAll()
        {
            var materias = _db.MateriasRepository.Get(includeProperties: "Carreras, Semestres, TipoAulaMaterias");
            return Mapper.Map<IEnumerable<MateriasDetailsDTO>>(materias);
        }

        // GET api/Materias/1
        [HttpGet("{codigo}", Name = "GetMateria")]
        public IActionResult Get(ushort codigo)
        {
            var materia = _db.MateriasRepository.Get(codigo);
            if (materia == null)
                return NotFound("No se encontro la materia buscada.");
            return new ObjectResult(Mapper.Map<MateriasDetailsDTO>(materia));
        }

        // PUT api/Materias/1
        [HttpPut("{codigo}")]
        public IActionResult Update(ushort codigo, [FromBody] MateriasDTO materia)
        {
            _db.MateriasRepository.Update(codigo, Mapper.Map<MateriasDTO, Materias>(materia));
            bool result = _db.Save();
            if (!result)
                return NotFound("No se encontro la materia a actualizar.");
            return new NoContentResult();
        }
    }
}