using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Schedule.API.Filters;
using Schedule.API.Models;
using Schedule.API.Models.Repositories;
using Schedule.Entities;
using System.Collections.Generic;
using System;

namespace Schedule.API.Controllers
{
    [Route("api/[controller]")]
    [GlobalAttibute]
    [AuthenticateAttribute]
    public class PeriodoCarreraController : Controller
    {
        private readonly PeriodoCarreraRepository _pcr = new PeriodoCarreraRepository();

        // POST api/PeriodoCarrera
        [HttpPost]
        [AuthorizationAttribute(Entities.Privilegios.Administrador)]
        public IActionResult Create([FromBody] PeriodoCarreraDTO periodo)
        {
            SetPeriodoDefaults(periodo);
            bool result = _pcr.Create(Mapper.Map<PeriodoCarreraDTO, PeriodoCarrera>(periodo));
            if (!result)
                return StatusCode(500);
            return CreatedAtRoute("GetPeriodoCarrera", new { id = 0 }, periodo);
        }

        // DELETE api/PeriodoCarrera/1
        [HttpDelete("{id}")]
        [AuthorizationAttribute(Entities.Privilegios.Administrador)]
        public IActionResult Delete(int id)
        {
            bool result = _pcr.Delete(id);
            if (!result)
                return StatusCode(500);
            return new NoContentResult();
        }

        // GET api/PeriodoCarrera
        [HttpGet]
        [AuthorizationAttribute(Entities.Privilegios.Administrador)]
        public IEnumerable<PeriodoCarreraDTO> GetAll()
        {
            return _pcr.Get();
        }

        // GET api/PeriodoCarrera/Current
        [HttpGet("Current")]
        public PeriodoCarreraDTO GetCurrentPeriodo()
        {
            return _pcr.GetCurrentPeriodo();
        }

        // GET api/PeriodoCarrera/1
        [HttpGet("{id}", Name = "GetPeriodoCarrera")]
        public IActionResult Get(int id)
        {
            var periodo = _pcr.Get(id);
            if (periodo == null)
                return NotFound("No se encontro el periodo academico buscado.");
            return new ObjectResult(periodo);
        }

        /// <summary>
        /// Actualiza la fecha de creacion y si el status es activo
        /// setea los demas periodos a inactivos
        /// </summary>
        /// <param name="periodo">Objeto PeriodoCarreraDTO</param>
        private void SetPeriodoDefaults(PeriodoCarreraDTO periodo)
        {
            if (periodo.Status)
            {
                _pcr.UpdateAllCurrentStatus();
            }
            periodo.FechaCreacion = DateTime.Now;
        }

        // PUT api/PeriodoCarrera/1
        [HttpPut("{id}")]
        [AuthorizationAttribute(Entities.Privilegios.Administrador)]
        public IActionResult Update(int id, [FromBody] PeriodoCarreraDTO periodo)
        {
            SetPeriodoDefaults(periodo);
            periodo.IdPeriodo = id;
            bool result = _pcr.Update(Mapper.Map<PeriodoCarreraDTO, PeriodoCarrera>(periodo));
            if (!result)
                return StatusCode(500);
            return new NoContentResult();
        }
    }
}
