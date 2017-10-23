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
    public class ProfesorMateriaController : Controller
    {
        private readonly ProfesorMateriaRepository _db = new ProfesorMateriaRepository();
        // POST api/ProfesorMateria/Create
        [HttpPost("Create")]
        //[AuthenticateAttribute].
        //[AuthorizationAttribute(Privilegios.Administrador)]
        public bool Create([FromBody] ProfesorMateriaDTO pm)
        {
            return _db.Create(Mapper.Map<ProfesorMateriaDTO, ProfesoresMaterias>(pm));
        }

        // DELETE api/ProfesorMateria/Delete/10
        [HttpDelete("Delete/{id}")]
        //[AuthenticateAttribute]
        //[AuthorizationAttribute(Privilegios.Administrador)]
        public bool Delete(int id)
        {
            return _db.Delete(id);
        }

        // GET api/ProfesorMateria/GetAll
        [HttpGet("GetAll")]
        //[AuthenticateAttribute]
        public IEnumerable<ProfesorMateriaDetailsDTO> GetAll()
        {
            return _db.Get();
        }
    }
}