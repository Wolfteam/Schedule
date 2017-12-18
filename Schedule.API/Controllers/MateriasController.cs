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
        private readonly UnitOfWork _db = new UnitOfWork();

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

        // GET api/Materias
        [HttpGet]
        public IEnumerable<MateriasDetailsDTO> GetAll()
        {
            var materias = _db.MateriasRepository.GetAll();
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
        [HttpPut("{id}")]
        public IActionResult Update(ushort codigo, [FromBody] MateriasDTO materia)
        {
            _db.MateriasRepository.Update(codigo, Mapper.Map<MateriasDTO, Materias>(materia));
            bool result = _db.Save();
            if (!result)
                return NotFound("No se encontro la materia a actualizar.");
            return new NoContentResult();
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}