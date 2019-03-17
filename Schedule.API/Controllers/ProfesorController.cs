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
    [Authorize]
    public class ProfesorController : BaseController
    {
        public ProfesorController(IUnitOfWork uow, IMapper mapper)
            : base(uow, mapper)
        {
        }

        // POST api/Profesor
        [Authorize(Roles = Roles.ADMINISTRADOR)]
        [HttpPost]
        public IActionResult Create([FromBody] ProfesorDTO profesor)
        {
            if (ProfesorExists(profesor.Cedula))
                return BadRequest($"El profesor {profesor.Cedula} ya existe");
            _db.ProfesorRepository.Add(_mapper.Map<ProfesorDTO, Profesores>(profesor));
            bool result = _db.Save();
            if (!result)
                return StatusCode(500, $"Ocurrio un error al actualzar el profesor {profesor.Cedula}");
            return CreatedAtRoute("GetProfesor", new { cedula = profesor.Cedula }, profesor);
        }

        // DELETE api/Profesor/21255727
        [Authorize(Roles = Roles.ADMINISTRADOR)]
        [HttpDelete("{cedula}")]
        public IActionResult Delete(uint cedula)
        {
            _db.ProfesorRepository.Remove(cedula);
            bool result = _db.Save();
            if (!result)
                return StatusCode(500, $"Ocurrio un error al borrar el profesor {cedula}");
            return new NoContentResult();
        }

        // DELETE api/Profesor?cedulas=1,2,3,4
        [Authorize(Roles = Roles.ADMINISTRADOR)]
        [HttpDelete]
        public IActionResult Delete([FromQuery] string cedulas)
        {
            uint[] profesoresToRemove = cedulas.Split(",").Select(value => uint.Parse(value.Trim())).ToArray();
            foreach (var profesor in profesoresToRemove)
                _db.ProfesorRepository.Remove(profesor);
            bool result = _db.Save();
            if (!result)
                return StatusCode(500, $"Ocurrio un error al borrar los profesores {cedulas}");
            return new NoContentResult();
        }

        // GET api/Profesor
        [Authorize(Roles = Roles.ADMINISTRADOR)]
        [HttpGet]
        public IEnumerable<ProfesorDetailsDTO> GetAll()
        {
            var profesores = _db.ProfesorRepository.Get(includeProperties: "PrioridadProfesor");
            return _mapper.Map<IEnumerable<ProfesorDetailsDTO>>(profesores);
        }

        // GET api/Profesor/1
        [Authorize(Roles = Roles.ADMINISTRADOR + ", " + Roles.PROFESOR)]
        [HttpGet("{cedula}", Name = "GetProfesor")]
        public IActionResult Get(uint cedula)
        {
            var profesor = _db.ProfesorRepository.Get(cedula);
            if (profesor == null)
                return NotFound("No se encontro el profesor buscado.");
            return new ObjectResult(_mapper.Map<ProfesorDetailsDTO>(profesor));
        }

        // PUT api/Profesor/21255727
        [Authorize(Roles = Roles.ADMINISTRADOR)]
        [HttpPut("{cedula}")]
        public IActionResult Update(uint cedula, [FromBody] ProfesorDTO profesor)
        {
            if (!ProfesorExists(cedula) || (cedula != profesor.Cedula && ProfesorExists(profesor.Cedula)))
                return NotFound($"El profesor {cedula} no existe o la ci:{profesor.Cedula} ya existe");
            _db.ProfesorRepository.Update(cedula, _mapper.Map<ProfesorDTO, Profesores>(profesor));
            bool result = _db.Save();
            if (!result)
                return StatusCode(500, $"Ocurrio un error al actualizar el profesor {profesor.Cedula}.");
            return new NoContentResult();
        }

        private bool ProfesorExists(uint cedula)
        {
            return _db.ProfesorRepository.Exists(prof => prof.Cedula == cedula);
        }
    }
}