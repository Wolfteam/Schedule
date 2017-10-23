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
    public class MateriasController : Controller
    {
        private readonly MateriasRepository _db = new MateriasRepository();

        // POST api/Materias/Create
        [HttpPost("Create")]
        //[AuthenticateAttribute].
        //[AuthorizationAttribute(Privilegios.Administrador)]
        public bool Create([FromBody] MateriasDTO materia)
        {
            return _db.Create(Mapper.Map<MateriasDTO, Materias>(materia));
        }

        // DELETE api/Materias/Delete/34052
        [HttpDelete("Delete/{codigo}")]
        //[AuthenticateAttribute]
        //[AuthorizationAttribute(Privilegios.Administrador)]
        public bool Delete(int codigo)
        {
            return _db.Delete(codigo);
        }

        // GET api/Materias/GetAll
        [HttpGet("GetAll")]
        //[AuthenticateAttribute]
        public IEnumerable<MateriasDetailsDTO> GetAll()
        {
            return _db.Get();
        }

        // GET api/Materias/Get/1
        [HttpGet("Get/{codigo}")]
        //[AuthenticateAttribute]
        public MateriasDetailsDTO Get(int codigo)
        {
            return _db.Get(codigo);
        }

        // PUT api/Materias/Update/1
        [HttpPut("Update/{id}")]
        //[AuthenticateAttribute]
        public bool Update(int id, [FromBody] MateriasDTO materia)
        {
            return _db.Update(id, Mapper.Map<MateriasDTO, Materias>(materia));
        }
    }
}