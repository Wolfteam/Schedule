using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Schedule.API.Models;
using Schedule.Entities;
using System.Collections.Generic;
using System;
using System.Linq;
using Schedule.API.Models.Repositories;

namespace Schedule.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = Roles.ADMINISTRADOR)]
    public class PeriodoCarreraController : BaseController
    {
        public PeriodoCarreraController(IUnitOfWork uow, IMapper mapper)
            : base(uow, mapper)
        {
        }

        // POST api/PeriodoCarrera
        [HttpPost]
        public IActionResult Create([FromBody] PeriodoCarreraDTO periodo)
        {
            SetPeriodoDefaults(periodo);
            _db.PeriodoCarreraRepository.Add(_mapper.Map<PeriodoCarreraDTO, PeriodoCarrera>(periodo));
            bool result = _db.Save();
            if (!result)
                return StatusCode(500, $"Ocurrio un error al crear el periodo {periodo.NombrePeriodo}");
            return CreatedAtRoute("GetPeriodoCarrera", new { id = 0 }, periodo);
        }

        // DELETE api/PeriodoCarrera/1
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _db.PeriodoCarreraRepository.Remove(id);
            bool result = _db.Save();
            if (!result)
                return StatusCode(500, $"Ocurrio un error al borrar el periodo {id}");
            return new NoContentResult();
        }

        // DELETE api/PeriodoCarrera?idPeriodos=1,2,3,4
        [HttpDelete]
        public IActionResult Delete([FromQuery] string idPeriodos)
        {
            int[] periodosToRemove = idPeriodos.Split(",").Select(value => int.Parse(value.Trim())).ToArray();
            foreach (var periodo in periodosToRemove)
                _db.PeriodoCarreraRepository.Remove(periodo);
            bool result = _db.Save();
            if (!result)
                return StatusCode(500, $"Ocurrio un error al borrar los periodos {idPeriodos}");
            return new NoContentResult();
        }

        // GET api/PeriodoCarrera
        [HttpGet]
        public IEnumerable<PeriodoCarreraDTO> GetAll()
        {
            var periodosCarrera = _db.PeriodoCarreraRepository.GetAll();
            return _mapper.Map<IEnumerable<PeriodoCarreraDTO>>(periodosCarrera);
        }

        // GET api/PeriodoCarrera/Current
        [HttpGet("Current")]
        [AllowAnonymous]
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
            return new ObjectResult(_mapper.Map<PeriodoCarreraDTO>(periodo));
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
        public IActionResult Update(int id, [FromBody] PeriodoCarreraDTO periodo)
        {
            if (!PeriodoExists(id))
                return NotFound($"El periodo academico no existe {id}");
            SetPeriodoDefaults(periodo);
            periodo.IdPeriodo = id;
            _db.PeriodoCarreraRepository.Update(_mapper.Map<PeriodoCarrera>(periodo));
            bool result = _db.Save();
            if (!result)
                return StatusCode(500, $"Ocurrio un error al actualizar el periodo {periodo.NombrePeriodo}");
            return new NoContentResult();
        }

        private bool PeriodoExists(int idPeriodo)
        {
            return _db.PeriodoCarreraRepository.Exists(pc => pc.IdPeriodo == idPeriodo);
        }
    }
}
