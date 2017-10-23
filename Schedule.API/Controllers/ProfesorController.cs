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
    [GlobalAttibute]
    public class ProfesorController : Controller
    {
        private readonly ProfesorRepository _db = new ProfesorRepository();

        // POST api/Profesor/Create
        [HttpPost("Create")]
        //[AuthenticateAttribute].
        //[AuthorizationAttribute(Privilegios.Administrador)]
        public bool Create([FromBody] ProfesorDTO profesor)
        {
            return _db.Create(Mapper.Map<ProfesorDTO, Profesores>(profesor));
        }

        // DELETE api/Profesor/Delete/21255727
        [HttpDelete("Delete/{cedula}")]
        //[AuthenticateAttribute]
        //[AuthorizationAttribute(Privilegios.Administrador)]
        public bool Delete(int cedula)
        {
            return _db.Delete(cedula);
        }

        // GET api/Profesor/GetAll
        [HttpGet("GetAll")]
        //[AuthenticateAttribute]
        public IEnumerable<ProfesorDetailsDTO> GetAll()
        {
            return _db.Get();
        }

        // GET api/Profesor/Get/1
        [HttpGet("Get/{cedula}")]
        //[AuthenticateAttribute]
        public ProfesorDetailsDTO Get(int cedula)
        {
            return _db.Get(cedula);
        }

        // PUT api/Profesor/Update/21255727
        [HttpPut("Update/{cedula}")]
        public bool Update(int cedula, [FromBody] ProfesorDTO profesor)
        {
            return _db.Update(cedula, Mapper.Map<ProfesorDTO, Profesores>(profesor));
        }
    }
}