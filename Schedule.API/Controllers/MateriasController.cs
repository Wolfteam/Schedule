using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Schedule.API.Models;
using Schedule.API.Models.Repositories;
using Schedule.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Schedule.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = Roles.ADMINISTRADOR)]
    public class MateriasController : BaseController
    {
        public MateriasController(IUnitOfWork uow, IMapper mapper)
            : base(uow, mapper)
        {
        }

        // POST api/Materias
        [HttpPost]
        public IActionResult Create([FromBody] MateriasDTO materia)
        {
            if (MateriaExists(materia.Codigo))
                return BadRequest($"La materia {materia.Codigo} ya existe");
            _db.MateriasRepository.Add(_mapper.Map<MateriasDTO, Materias>(materia));
            bool result = _db.Save();
            if (!result)
                return StatusCode(500, $"Ocurrio un error al crear la materia {materia.Codigo}");
            return CreatedAtRoute("GetMateria", new { codigo = materia.Codigo }, materia);
        }

        // DELETE api/Materias/34052
        [HttpDelete("{codigo}")]
        public IActionResult Delete(ushort codigo)
        {
            _db.MateriasRepository.Remove(codigo);
            bool result = _db.Save();
            if (!result)
                return StatusCode(500, $"Ocurrio un error al borrar la materia {codigo}");
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
                return StatusCode(500, $"Ocurrio un error al borrar las materias {codigos}");
            return new NoContentResult();
        }

        // GET api/Materias
        [HttpGet]
        public IEnumerable<MateriasDetailsDTO> GetAll()
        {
            var materias = _db.MateriasRepository.Get(includeProperties: "Carreras, Semestres, TipoAulaMaterias");
            return _mapper.Map<IEnumerable<MateriasDetailsDTO>>(materias);
        }

        // GET api/Materias/1
        [HttpGet("{codigo}", Name = "GetMateria")]
        public IActionResult Get(ushort codigo)
        {
            var materia = _db.MateriasRepository.Get(codigo);
            if (materia == null)
                return NotFound("No se encontro la materia buscada.");
            return new ObjectResult(_mapper.Map<MateriasDetailsDTO>(materia));
        }

        // PUT api/Materias/1
        [HttpPut("{codigo}")]
        public IActionResult Update(ushort codigo, [FromBody] MateriasDTO materia)
        {
            if (!MateriaExists(codigo) || (codigo != materia.Codigo && MateriaExists(materia.Codigo)))
                return NotFound($"La materia {codigo} no existe o el codigo: {materia.Codigo} ya existe");
            _db.MateriasRepository.Update(codigo, _mapper.Map<Materias>(materia));
            bool result = _db.Save();
            if (!result)
                return StatusCode(500, $"Ocurrio un error al tratar de actualizar la materia {codigo}.");
            return new NoContentResult();
        }

        private bool MateriaExists(ushort codigo)
        {
            return _db.MateriasRepository.Exists(materia => materia.Codigo == codigo);
        }
    }
}

