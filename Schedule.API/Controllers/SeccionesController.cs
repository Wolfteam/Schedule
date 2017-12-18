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
    [AuthenticateAttribute]
    [AuthorizationAttribute(Entities.Privilegios.Administrador)]
    public class SeccionesController : Controller
    {
        private readonly UnitOfWork _db = new UnitOfWork();

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

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}
