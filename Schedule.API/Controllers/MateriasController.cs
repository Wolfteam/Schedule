using Microsoft.AspNetCore.Mvc;
using Schedule.API.Filters;
using Schedule.BLL;
using Schedule.Entities;
using System.Collections.Generic;

namespace Schedule.API.Controllers
{
    [Route("api/[controller]")]
    [GlobalAttibute]
    public class MateriasController : Controller
    {
        // POST api/Materias/Create
        [HttpPost("Create")]
        //[AuthenticateAttribute].
        //[AuthorizationAttribute(Privilegios.Administrador)]
        public bool Create([FromBody] Materias materia)
        {
            return new MateriasBLL().Create(materia);
        }

        // DELETE api/Materias/Delete/34052
        [HttpDelete("Delete/{codigo}")]
        //[AuthenticateAttribute]
        //[AuthorizationAttribute(Privilegios.Administrador)]
        public bool Delete(int codigo)
        {
            return new MateriasBLL().Delete(codigo);
        }

        // GET api/Materias/GetAll
        [HttpGet("GetAll")]
        //[AuthenticateAttribute]
        public List<Materias> GetAll()
        {
            return new MateriasBLL().GetAll();
        }

        // GET api/Materias/Get/1
        [HttpGet("Get/{codigo}")]
        //[AuthenticateAttribute]
        public Materias Get(int codigo)
        {
            return new MateriasBLL().Get(codigo);
        }
    }
}