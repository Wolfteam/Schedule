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

        // POST api/Secciones
        [HttpPost]
        //[AuthenticateAttribute].
        //[AuthorizationAttribute(Privilegios.Administrador)]
        public IActionResult Create([FromBody] SeccionesDTO seccion)
        {
            bool result = _db.Create(Mapper.Map<SeccionesDTO, Secciones>(seccion));
            if (!result)
                return StatusCode(500);
            return CreatedAtRoute("GetSeccion", new { codigo = seccion.Codigo }, seccion);
        }

        // DELETE api/Secciones/44605
        [HttpDelete("{codigo}")]
        //[AuthenticateAttribute]
        //[AuthorizationAttribute(Privilegios.Administrador)]
        public IActionResult Delete(int codigo)
        {
            bool result = _db.Delete(codigo);
            if (!result)
                return StatusCode(404);
            return new NoContentResult();
        }

        // GET api/Secciones
        [HttpGet]
        //[AuthenticateAttribute]
        public IEnumerable<SeccionesDetailsDTO> GetAll()
        {
            return _db.Get();
        }

        // GET api/Secciones/44056
        [HttpGet("{codigo}", Name = "GetSeccion")]
        //[AuthenticateAttribute]
        public IActionResult Get(int codigo)
        {
            var seccion = _db.Get(codigo);
            if (seccion == null)
                return NotFound();
            return new ObjectResult(seccion);
        }

        // PUT api/Secciones/44056
        [HttpPut("{codigo}")]
        public IActionResult Update(int codigo, [FromBody] SeccionesDTO seccion)
        {
            bool result = _db.Update(codigo, Mapper.Map<SeccionesDTO, Secciones>(seccion));
            if (!result)
                return StatusCode(404);
            return new NoContentResult();
        }
    }
}
