using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Schedule.API.Models;
using Schedule.Entities;
using System.Collections.Generic;

namespace Schedule.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrador")]
    public class AulasController : BaseController
    {
        public AulasController(HorariosContext context) 
            : base(context)
        {
        }

        // POST api/Aulas
        [HttpPost]
        public IActionResult Create([FromBody] AulasDTO aula)
        {
            _db.AulasRepository.Add(Mapper.Map<AulasDTO, Aulas>(aula));
            bool result = _db.Save();
            if (!result)
                return StatusCode(500);
            return CreatedAtRoute("GetAula", new { id = aula.IdAula }, aula);
        }

        // DELETE api/Aulas/1
        [HttpDelete("{id}")]
        public IActionResult Delete(byte id)
        {
            _db.AulasRepository.Remove(id);
            bool result = _db.Save();
            if (!result)
                return NotFound("No existe el aula a borrar.");
            return new NoContentResult();
        }

        // GET api/Aulas
        [HttpGet]
        public IEnumerable<AulasDetailsDTO> GetAll()
        {
            var aulas = _db.AulasRepository.Get(includeProperties: "TipoAulaMateria");
            return Mapper.Map<IEnumerable<Aulas>, IEnumerable<AulasDetailsDTO>>(aulas);
        }

        // GET api/Aulas/Tipo/2/Capacidad/20
        [HttpGet("Tipo/{idTipo}/Capacidad/{capacidad}")]
        public IEnumerable<AulasDTO> GetByTipoCapacidad(byte idTipo, byte capacidad)
        {
            return _db.AulasRepository.GetByTipoCapacidad(idTipo, capacidad);
        }


        // GET api/Aulas/1
        [HttpGet("{id}", Name = "GetAula")]
        public IActionResult Get(byte id)
        {
            var aula = _db.AulasRepository.Get(id);
            if (aula == null)
                return NotFound("No se encontro el aula buscada.");
            return new ObjectResult(Mapper.Map<AulasDetailsDTO>(aula));
        }

        // PUT api/Aulas/1
        [HttpPut("{id}")]
        public IActionResult Update(byte id, [FromBody] AulasDTO aula)
        {
            _db.AulasRepository.Update(id, Mapper.Map<AulasDTO, Aulas>(aula));
            bool result = _db.Save();
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
            var result = _db.AulasRepository.GetTest(searchBy, take, skip, sortBy, sortDir, out filteredResultsCount, out totalResultsCount);
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
