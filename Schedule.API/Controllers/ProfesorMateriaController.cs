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
        
        // POST api/ProfesorMateria
        [HttpPost]
        //[AuthenticateAttribute].
        //[AuthorizationAttribute(Privilegios.Administrador)]
        public IActionResult Create([FromBody] ProfesorMateriaDTO pm)
        {
            bool result = _db.Create(Mapper.Map<ProfesorMateriaDTO, ProfesoresMaterias>(pm));
            if (!result)
                return StatusCode(500);
            //return CreatedAtRoute("GetProfesorMateria", new { id = pm.Id }, pm);
            return StatusCode(200);
        }

        // DELETE api/ProfesorMateria/10
        [HttpDelete("{id}")]
        //[AuthenticateAttribute]
        //[AuthorizationAttribute(Privilegios.Administrador)]
        public IActionResult Delete(int id)
        {
            bool result = _db.Delete(id);
            if (!result)
                return StatusCode(404);
            return new NoContentResult();
        }

        // GET api/ProfesorMateria
        [HttpGet]
        //[AuthenticateAttribute]
        public IEnumerable<ProfesorMateriaDetailsDTO> GetAll()
        {
            return _db.Get();
        }
    }
}