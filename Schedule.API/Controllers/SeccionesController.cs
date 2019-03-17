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
    public class SeccionesController : BaseController
    {
        public SeccionesController(IUnitOfWork uow, IMapper mapper) 
            : base(uow, mapper)
        {
        }

        // POST api/Secciones
        [HttpPost]
        public IActionResult Create([FromBody] SeccionesDTO seccion)
        {
            if (SeccionExists(seccion.Codigo))
                return BadRequest($"La seccion ya existe {seccion.Codigo}");
            seccion.IdPeriodo = _db.PeriodoCarreraRepository.GetCurrentPeriodo().IdPeriodo;
            _db.SeccionesRepository.Add(_mapper.Map<SeccionesDTO, Secciones>(seccion));
            bool result = _db.Save();
            if (!result)
                return StatusCode(500, $"Ocurrio un error al crear la seccion {seccion.Codigo}");
            return CreatedAtRoute("GetSeccion", new { codigo = seccion.Codigo }, seccion);
        }

        // DELETE api/Secciones/44605
        [HttpDelete("{codigo}")]
        public IActionResult Delete(ushort codigo)
        {
            _db.SeccionesRepository.Remove(codigo);
            bool result = _db.Save();
            if (!result)
                return StatusCode(500, $"Ocurrio un error al borrar la seccion {codigo}");
            return new NoContentResult();
        }

        // DELETE api/Secciones?codigos=1,2,3,4
        [HttpDelete]
        public IActionResult Delete([FromQuery] string codigos)
        {
            ushort[] seccionesToRemove = codigos.Split(",").Select(value => ushort.Parse(value.Trim())).ToArray();
            foreach (var seccion in seccionesToRemove)
                _db.SeccionesRepository.Remove(seccion);
            bool result = _db.Save();
            if (!result)
                return StatusCode(500, $"Ocurrio un error al borrar las secciones {codigos}");
            return new NoContentResult();
        }

        // GET api/Secciones
        [HttpGet]
        public IEnumerable<SeccionesDetailsDTO> GetAll()
        {
            return _db.SeccionesRepository.GetAllCurrent();
        }

        // GET api/Secciones/44056
        [HttpGet("{codigo}", Name = "GetSeccion")]
        public IActionResult Get(ushort codigo)
        {
            var seccion = _db.SeccionesRepository.GetCurrent(codigo);
            if (seccion == null)
                return NotFound("No se encontro la seccion buscada.");
            return new ObjectResult(seccion);
        }

        // PUT api/Secciones/44056
        [HttpPut("{codigo}")]
        public IActionResult Update(ushort codigo, [FromBody] SeccionesDTO seccion)
        {
            if (!SeccionExists(codigo) || (codigo != seccion.Codigo && SeccionExists(seccion.Codigo)))
                return NotFound($"No existe la materia {codigo} o ya existe una seccion para {seccion.Codigo}");
            seccion.IdPeriodo = _db.PeriodoCarreraRepository.GetCurrentPeriodo().IdPeriodo;
            _db.SeccionesRepository.Update(codigo, _mapper.Map<SeccionesDTO, Secciones>(seccion));
            bool result = _db.Save();
            if (!result)
                return StatusCode(500, $"Ocurrio un error al actualizar la seccion {codigo}");
            return new NoContentResult();
        }

        private bool SeccionExists(ushort codigo)
        {
            return _db.SeccionesRepository.GetCurrent(codigo) != null;
        }
    }
}
