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
    public class AulasController : Controller
    {
        private readonly AulasRepository _db = new AulasRepository();

        // POST api/Aulas/Create
        [HttpPost("Create")]
        //[AuthenticateAttribute]
        //[AuthorizationAttribute(Entities.Privilegios.Administrador)]
        public bool Create([FromBody] AulasDTO aula)
        {
            return _db.Create(Mapper.Map<AulasDTO, Aulas>(aula));
        }

        // DELETE api/Aulas/Delete/1
        [HttpDelete("Delete/{id}")]
        //[AuthenticateAttribute]
        //[AuthorizationAttribute(Entities.Privilegios.Administrador)]
        public bool Delete(int id)
        {
            return _db.Delete(id);
        }

        // GET api/Aulas/GetAll
        [HttpGet("GetAll")]
        //[AuthenticateAttribute]
        public IEnumerable<AulasDetailsDTO> GetAll()
        {
            return _db.Get();
        }

        // GET api/Aulas/Get/1
        [HttpGet("Get/{id}")]
        //[AuthenticateAttribute]
        public AulasDetailsDTO Get(int id)
        {
            return _db.Get(id);
        }

        // PUT api/Aulas/Update/1
        [HttpPut("Update/{id}")]
        //[AuthenticateAttribute]
        //[AuthorizationAttribute(Entities.Privilegios.Administrador)]
        public bool Update(int id, [FromBody] AulasDTO aula)
        {
            return _db.Update(id, Mapper.Map<AulasDTO, Aulas>(aula));
        }
    }
}
