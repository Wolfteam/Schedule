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
    public class MateriasController : Controller
    {
        private readonly MateriasRepository _db = new MateriasRepository();

        // POST api/Materias
        [HttpPost]
        public IActionResult Create([FromBody] MateriasDTO materia)
        {
            bool result = _db.Create(Mapper.Map<MateriasDTO, Materias>(materia));
            if (!result)
                return StatusCode(500);
            return CreatedAtRoute("GetMateria", new { codigo = materia.Codigo }, materia);
        }

        // DELETE api/Materias/34052
        [HttpDelete("{codigo}")]
        public IActionResult Delete(int codigo)
        {
            bool result = _db.Delete(codigo);
            if (!result)
                return NotFound("No se encontro la materia a borrar.");
            return new NoContentResult();
        }

        // GET api/Materias
        [HttpGet]
        public IEnumerable<MateriasDetailsDTO> GetAll()
        {
            return _db.Get();
        }

        // GET api/Materias/1
        [HttpGet("{codigo}", Name = "GetMateria")]
        public IActionResult Get(int codigo)
        {
            var materia = _db.Get(codigo);
            if (materia == null)
                return NotFound("No se encontro la materia buscada.");
            return new ObjectResult(materia);
        }

        // PUT api/Materias/1
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] MateriasDTO materia)
        {
            bool result = _db.Update(id, Mapper.Map<MateriasDTO, Materias>(materia));
            if (!result)
                return NotFound("No se encontro la materia a actualizar.");
            return new NoContentResult();
        }
    }
}