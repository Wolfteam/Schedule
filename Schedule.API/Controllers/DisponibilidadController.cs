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
    [Authorize(Roles = Roles.ADMINISTRADOR + ", " + Roles.PROFESOR)]
    public class DisponibilidadController : BaseController
    {
        public DisponibilidadController(HorariosContext context) 
            : base(context)
        {
        }

        // POST api/Disponibilidad
        [HttpPost]
        public IActionResult Create([FromBody] IEnumerable<DisponibilidadProfesorDTO> disponibilidad)
        {
            int idPeriodoActual = _db.PeriodoCarreraRepository.GetCurrentPeriodo().IdPeriodo;
            disponibilidad.ToList().ForEach(d => d.IdPeriodo = idPeriodoActual);

            _db.DisponibilidadProfesorRepository.AddRange(Mapper.Map<IEnumerable<DisponibilidadProfesores>>(disponibilidad));
            bool result = _db.Save();
            if (!result)
                return StatusCode(500);
            return CreatedAtRoute("GetDisponibilidad", new { cedula = disponibilidad.FirstOrDefault().Cedula }, disponibilidad);
        }

        // DELETE api/Disponibilidad/21255727
        [HttpDelete("{cedula}")]
        public IActionResult Delete(uint cedula)
        {
            _db.DisponibilidadProfesorRepository.RemoveByCedula(cedula);
            bool result = _db.Save();
            if (!result)
                return NotFound("No se encontro la disponibilidad para la cedula indicada.");
            return new NoContentResult();
        }

        //TODO: Terminar esto
        // DELETE api/Disponibilidad
        // [HttpDelete]
        // public IActionResult Delete()
        // {
        //     _db.DisponibilidadProfesorRepository.Remove()
        //     bool result = _db.Save() == -1 ? false : true;
        //     if (!result)
        //         return StatusCode(500);
        //     return new NoContentResult();
        // }

        // GET api/Disponibilidad/21255727
        [HttpGet("{cedula}", Name = "GetDisponibilidad")]
        public IActionResult Get(int cedula)
        {
            var disponibilidad = _db.DisponibilidadProfesorRepository.GetByCedula(cedula);
            disponibilidad.HorasACumplir = _db.ProfesorRepository.GetHorasACumplir(cedula);
            return new ObjectResult(disponibilidad);
        }

        [HttpGet("{cedula}/{idDia}")]
        public IActionResult Get(int cedula, byte idDia)
        {
            var disponibilidad = _db.DisponibilidadProfesorRepository.GetByCedulaDia(cedula, idDia);
            if (disponibilidad.Disponibilidad != null)
                return new ObjectResult(disponibilidad);
            disponibilidad.HorasACumplir = _db.ProfesorRepository.GetHorasACumplir(cedula);
            return new ObjectResult(disponibilidad);
        }

        [HttpGet("HorasAsignadasACumplir/{cedula}")]
        public IActionResult GetHorasAsignadasACumplir(int cedula)
        {
            var disp = new DisponibilidadProfesorDetailsDTO()
            {
                Cedula = (uint)cedula,
                HorasACumplir = _db.ProfesorRepository.GetHorasACumplir(cedula),
                HorasAsignadas = _db.DisponibilidadProfesorRepository.GetHorasAsignadas(cedula)
            };
            return new ObjectResult(disp);
        }
    }
}