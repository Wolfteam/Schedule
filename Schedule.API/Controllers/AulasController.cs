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
    [AuthenticateAttribute]
    [AuthorizationAttribute(Entities.Privilegios.Administrador)]
    public class AulasController : Controller
    {
        private readonly AulasRepository _db = new AulasRepository();

        // POST api/Aulas
        [HttpPost]
        public IActionResult Create([FromBody] AulasDTO aula)
        {
            bool result = _db.Create(Mapper.Map<AulasDTO, Aulas>(aula));
            if (!result)
                return StatusCode(500);
            return CreatedAtRoute("GetAula", new { id = aula.IdAula }, aula);
        }

        // DELETE api/Aulas/1
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool result = _db.Delete(id);
            if (!result)
                return NotFound("No existe el aula a borrar.");
            return new NoContentResult();
        }

        // GET api/Aulas
        [HttpGet]
        public IEnumerable<AulasDetailsDTO> GetAll()
        {
            return _db.Get();
        }

        // GET api/Aulas/Tipo/2/Capacidad/20
        [HttpGet("Tipo/{idTipo}/Capacidad/{capacidad}")]
        public IEnumerable<AulasDTO> GetByTipoCapacidad(byte idTipo, byte capacidad)
        {
            return _db.GetByTipoCapacidad(idTipo, capacidad);
        }


        // GET api/Aulas/1
        [HttpGet("{id}", Name = "GetAula")]
        public IActionResult Get(int id)
        {
            var aula = _db.Get(id);
            if (aula == null)
                return NotFound("No se encontro el aula buscada.");        
            return new ObjectResult(aula);
        }

        // PUT api/Aulas/1
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] AulasDTO aula)
        {
            bool result = _db.Update(id, Mapper.Map<AulasDTO, Aulas>(aula));
            if (!result)
                return NotFound("No existe el aula a actualizar");
            return new NoContentResult();
        }

        #region Pruebas
        [HttpPost("GetTest")]
        public IActionResult GetTest(DataTableAjaxPostModel model)
        {
            var result = SearchAulas(model, out int filteredResultsCount, out int totalResultsCount);
            return Json(new
            {
                // this is what datatables wants sending back
                draw = model.Draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = result
            });
        }

        private List<AulasDetailsDTO> SearchAulas(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
        {
            var searchBy = model.Search?.Value;
            var take = model.Length;
            var skip = model.Start;

            string sortBy = "";
            bool sortDir = true;

            if (model.Order != null)
            {
                sortBy = model.Columns[model.Order[0].Column].Data;
                sortDir = model.Order[0].Dir.ToLower() == "asc";
            }

            // search the dbase taking into consideration table sorting and paging
            var result = _db.GetTest(searchBy, take, skip, sortBy, sortDir, out filteredResultsCount, out totalResultsCount);
            if (result == null)
            {
                // empty collection...
                return new List<AulasDetailsDTO>();
            }
            return result;
        }

        #endregion
    }
}
