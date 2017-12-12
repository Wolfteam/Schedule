using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Schedule.API.Filters;
using Schedule.API.Models;
using Schedule.API.Models.Repositories;
using Schedule.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Schedule.API.Controllers
{
    [Route("api/[controller]")]
    [GlobalAttibute]
    [AuthenticateAttribute]
    public class DisponibilidadController : Controller
    {
        private readonly DisponibilidadProfesorRepository _db = new DisponibilidadProfesorRepository();
        private readonly PeriodoCarreraRepository _pcr = new PeriodoCarreraRepository();

        // POST api/Disponibilidad
        [HttpPost]
        public IActionResult Create([FromBody] IEnumerable<DisponibilidadProfesorDTO> disponibilidad)
        {
            int idPeriodoActual = _pcr.GetCurrentPeriodo().IdPeriodo;
            disponibilidad.ToList().ForEach(d => d.IdPeriodo = idPeriodoActual);

            bool result = _db.Create(Mapper.Map<IEnumerable<DisponibilidadProfesorDTO>, IEnumerable<DisponibilidadProfesores>>(disponibilidad));
            if (!result)
                return StatusCode(500);
            return CreatedAtRoute("GetDisponibilidad", new { cedula = disponibilidad.FirstOrDefault().Cedula }, disponibilidad);
        }

        // DELETE api/Disponibilidad/21255727
        [HttpDelete("{cedula}")]
        public IActionResult Delete(uint cedula)
        {
            bool result = _db.Delete(cedula);
            if (!result)
                return NotFound("No se encontro la disponibilidad para la cedula indicada.");
            return new NoContentResult();
        }

        // DELETE api/Disponibilidad
        [HttpDelete]
        public IActionResult Delete()
        {
            bool result = _db.Delete();
            if (!result)
                return StatusCode(500);
            return new NoContentResult();
        }

        // GET api/Disponibilidad/21255727
        [HttpGet("{cedula}", Name = "GetDisponibilidad")]
        public IActionResult Get(int cedula)
        {
            //TODO: Aca deberia ir a pedir las horas a cumplir del profesor en el repo del mismo
            //seria weno aplicar el repo pattern de una vez pa no tener q crear una variable
            //para cada repo
            var disponibilidad = _db.Get(cedula);
            if (disponibilidad.Disponibilidad != null)
                return new ObjectResult(disponibilidad);
            ProfesorRepository pr = new ProfesorRepository();
            disponibilidad.Cedula = (uint)cedula;
            //asumo que la cedula existe
            disponibilidad.HorasACumplir = pr.GetHorasACumplir(cedula);
            return new ObjectResult(disponibilidad);
        }
    }
}