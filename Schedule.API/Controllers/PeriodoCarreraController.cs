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
    public class PeriodoCarreraController : BaseController
    {
        // POST api/PeriodoCarrera
        [HttpPost]
        [AuthorizationAttribute(Entities.Privilegios.Administrador)]
        public IActionResult Create([FromBody] PeriodoCarreraDTO periodo)
        {
            SetPeriodoDefaults(periodo);
            _db.PeriodoCarreraRepository.Add(Mapper.Map<PeriodoCarreraDTO, PeriodoCarrera>(periodo));
            bool result = _db.Save();
            if (!result)
                return StatusCode(500);
            return CreatedAtRoute("GetPeriodoCarrera", new { id = 0 }, periodo);
        }

        // DELETE api/PeriodoCarrera/1
        [HttpDelete("{id}")]
        [AuthorizationAttribute(Entities.Privilegios.Administrador)]
        public IActionResult Delete(int id)
        {
            _db.PeriodoCarreraRepository.Remove(id);
            bool result = _db.Save();
            if (!result)
                return StatusCode(500);
            return new NoContentResult();
        }

        // GET api/PeriodoCarrera
        [HttpGet]
        [AuthorizationAttribute(Entities.Privilegios.Administrador)]
        public IEnumerable<PeriodoCarreraDTO> GetAll()
        {
            var periodosCarrera =  _db.PeriodoCarreraRepository.GetAll();
            return Mapper.Map<IEnumerable<PeriodoCarreraDTO>>(periodosCarrera);
        }

        // GET api/PeriodoCarrera/Current
        [HttpGet("Current")]
        public PeriodoCarreraDTO GetCurrentPeriodo()
        {
            return _db.PeriodoCarreraRepository.GetCurrentPeriodo();
        }

        // GET api/PeriodoCarrera/1
        [HttpGet("{id}", Name = "GetPeriodoCarrera")]
        public IActionResult Get(int id)
        {
            var periodo = _db.PeriodoCarreraRepository.Get(id);
            if (periodo == null)
                return NotFound("No se encontro el periodo academico buscado.");
            return new ObjectResult(Mapper.Map<PeriodoCarreraDTO>(periodo));
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
                _db.PeriodoCarreraRepository.UpdateAllCurrentStatus();
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
            _db.PeriodoCarreraRepository.Update(Mapper.Map<PeriodoCarreraDTO, PeriodoCarrera>(periodo));
            bool result = _db.Save();
            if (!result)
                return StatusCode(500);
            return new NoContentResult();
        }
    }
}
