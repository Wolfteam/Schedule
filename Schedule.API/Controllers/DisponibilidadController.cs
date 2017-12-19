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
    [Authorize(Roles = "Administrador, Profesor")]
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
            if (disponibilidad.Disponibilidad != null)
                return new ObjectResult(disponibilidad);
            disponibilidad.Cedula = (uint)cedula;
            //asumo que la cedula existe
            disponibilidad.HorasACumplir = _db.ProfesorRepository.GetHorasACumplir(cedula);
            return new ObjectResult(disponibilidad);
        }
    }
}