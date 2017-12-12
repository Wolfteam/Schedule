using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Schedule.API.Models;
using Schedule.API.Models.Repositories;
using Schedule.Entities;
using System.Collections.Generic;
using Schedule.API.Filters;

namespace Schedule.API.Controllers
{
    [Route("api/[controller]")]
    [AuthenticateAttribute]
    [AuthorizationAttribute(Entities.Privilegios.Administrador)]
    public class SeccionesController : Controller
    {
        private readonly SeccionesRepository _db = new SeccionesRepository();
        private readonly PeriodoCarreraRepository pcr = new PeriodoCarreraRepository();

        // POST api/Secciones
        [HttpPost]
        public IActionResult Create([FromBody] SeccionesDTO seccion)
        {
            seccion.IdPeriodo = pcr.GetCurrentPeriodo().IdPeriodo;
            bool result = _db.Create(Mapper.Map<SeccionesDTO, Secciones>(seccion));
            if (!result)
                return StatusCode(500);
            return CreatedAtRoute("GetSeccion", new { codigo = seccion.Codigo }, seccion);
        }

        // DELETE api/Secciones/44605
        [HttpDelete("{codigo}")]
        public IActionResult Delete(int codigo)
        {
            bool result = _db.Delete(codigo);
            if (!result)
                return NotFound("No se encontro la seccion a borrar.");
            return new NoContentResult();
        }

        // GET api/Secciones
        [HttpGet]
        public IEnumerable<SeccionesDetailsDTO> GetAll()
        {
            return _db.Get();
        }

        // GET api/Secciones/44056
        [HttpGet("{codigo}", Name = "GetSeccion")]
        public IActionResult Get(int codigo)
        {
            var seccion = _db.Get(codigo);
            if (seccion == null)
                return NotFound("No se encontro la seccion buscada.");
            return new ObjectResult(seccion);
        }

        // PUT api/Secciones/44056
        [HttpPut("{codigo}")]
        public IActionResult Update(int codigo, [FromBody] SeccionesDTO seccion)
        {
            seccion.IdPeriodo = pcr.GetCurrentPeriodo().IdPeriodo;
            bool result = _db.Update(codigo, Mapper.Map<SeccionesDTO, Secciones>(seccion));
            if (!result)
                return NotFound("No se encontro la seccion a actualizar.");
            return new NoContentResult();
        }
    }
}
