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
    public class SeccionesController : BaseController
    {
        public SeccionesController(HorariosContext context) 
            : base(context)
        {
        }

        // POST api/Secciones
        [HttpPost]
        public IActionResult Create([FromBody] SeccionesDTO seccion)
        {
            seccion.IdPeriodo = _db.PeriodoCarreraRepository.GetCurrentPeriodo().IdPeriodo;
            _db.SeccionesRepository.Add(Mapper.Map<SeccionesDTO, Secciones>(seccion));
            bool result = _db.Save();
            if (!result)
                return StatusCode(500);
            return CreatedAtRoute("GetSeccion", new { codigo = seccion.Codigo }, seccion);
        }

        // DELETE api/Secciones/44605
        [HttpDelete("{codigo}")]
        public IActionResult Delete(ushort codigo)
        {
            _db.SeccionesRepository.Remove(codigo);
            bool result = _db.Save();
            if (!result)
                return NotFound("No se encontro la seccion a borrar.");
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
                return NotFound("No se encontro alguna seccion a borrar.");
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
            seccion.IdPeriodo = _db.PeriodoCarreraRepository.GetCurrentPeriodo().IdPeriodo;
            _db.SeccionesRepository.Update(codigo, Mapper.Map<SeccionesDTO, Secciones>(seccion));
            bool result = _db.Save();
            if (!result)
                return NotFound("No se encontro la seccion a actualizar.");
            return new NoContentResult();
        }
    }
}
