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
    public class MateriasController : Controller
    {
        private readonly MateriasRepository _db = new MateriasRepository();

        // POST api/Materias
        [HttpPost]
        //[AuthenticateAttribute].
        //[AuthorizationAttribute(Privilegios.Administrador)]
        public IActionResult Create([FromBody] MateriasDTO materia)
        {
            bool result = _db.Create(Mapper.Map<MateriasDTO, Materias>(materia));
            if (!result)
                return StatusCode(500);
            return CreatedAtRoute("GetMateria", new { codigo = materia.Codigo }, materia);
        }

        // DELETE api/Materias/34052
        [HttpDelete("{codigo}")]
        //[AuthenticateAttribute]
        //[AuthorizationAttribute(Privilegios.Administrador)]
        public IActionResult Delete(int codigo)
        {
            bool result = _db.Delete(codigo);
            if (!result)
                return StatusCode(404);
            return new NoContentResult();
        }

        // GET api/Materias
        [HttpGet]
        //[AuthenticateAttribute]
        public IEnumerable<MateriasDetailsDTO> GetAll()
        {
            return _db.Get();
        }

        // GET api/Materias/1
        [HttpGet("{codigo}", Name = "GetMateria")]
        //[AuthenticateAttribute]
        public IActionResult Get(int codigo)
        {
            var materia = _db.Get(codigo);
            if (materia == null)
                return NotFound();       
            return new ObjectResult(materia);
        }

        // PUT api/Materias/1
        [HttpPut("{id}")]
        //[AuthenticateAttribute]
        public IActionResult Update(int id, [FromBody] MateriasDTO materia)
        {
            bool result = _db.Update(id, Mapper.Map<MateriasDTO, Materias>(materia));
            if (!result)
                return StatusCode(404);
            return new NoContentResult();
        }
    }
}