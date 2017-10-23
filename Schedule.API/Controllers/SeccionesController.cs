using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Schedule.API.Models;
using Schedule.API.Models.Repositories;
using Schedule.Entities;
using System.Collections.Generic;

namespace Schedule.API.Controllers
{
    [Route("api/[controller]")]
    public class SeccionesController : Controller
    {
        private readonly SeccionesRepository _db = new SeccionesRepository();
        // POST api/Secciones/Create
        [HttpPost("Create")]
        //[AuthenticateAttribute].
        //[AuthorizationAttribute(Privilegios.Administrador)]
        public bool Create([FromBody] SeccionesDTO seccion)
        {
            return _db.Create(Mapper.Map<SeccionesDTO, Secciones>(seccion));
        }

        // DELETE api/Secciones/Delete/44605
        [HttpDelete("Delete/{codigo}")]
        //[AuthenticateAttribute]
        //[AuthorizationAttribute(Privilegios.Administrador)]
        public bool Delete(int codigo)
        {
            return _db.Delete(codigo);
        }

        // GET api/Secciones/GetAll
        [HttpGet("GetAll")]
        //[AuthenticateAttribute]
        public IEnumerable<SeccionesDetailsDTO> GetAll()
        {
            return _db.Get();
        }

        // GET api/Secciones/Get/44056
        [HttpGet("Get/{codigo}")]
        //[AuthenticateAttribute]
        public SeccionesDetailsDTO Get(int codigo)
        {
            return _db.Get(codigo);
        }

        // PUT api/Secciones/Update/44056
        [HttpPut("Update/{codigo}")]
        public bool Update(int codigo, [FromBody] SeccionesDTO seccion)
        {
            return _db.Update(codigo, Mapper.Map<SeccionesDTO, Secciones>(seccion));
        }
    }
}
