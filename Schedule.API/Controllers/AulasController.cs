using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Schedule.API.Models;
using Schedule.API.Models.Repositories;
using Schedule.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Schedule.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = Roles.ADMINISTRADOR)]
    public class AulasController : BaseController
    {
        public AulasController(IUnitOfWork uow, IMapper mapper)
            : base(uow, mapper)
        {
        }

        // POST api/Aulas
        [HttpPost]
        public IActionResult Create([FromBody] AulasDTO aula)
        {
            _db.AulasRepository.Add(_mapper.Map<AulasDTO, Aulas>(aula));
            bool result = _db.Save();
            if (!result)
                return StatusCode(500, $"Ocurrio un error al crear el aula {aula.NombreAula}");
            return CreatedAtRoute("GetAula", new { id = aula.IdAula }, aula);
        }

        // DELETE api/Aulas/1
        [HttpDelete("{id}")]
        public IActionResult Delete(byte id)
        {
            _db.AulasRepository.Remove(id);
            bool result = _db.Save();
            if (!result)
                return StatusCode(500, $"Ocurrio un error al borrar el aula {id}.");
            return new NoContentResult();
        }

        // DELETE api/Aulas?idAulas=1,2,3,4
        [HttpDelete]
        public IActionResult Delete([FromQuery] string idAulas)
        {
            byte[] aulasToRemove = idAulas.Split(",").Select(value => byte.Parse(value.Trim())).ToArray();
            foreach (var aula in aulasToRemove)
                _db.AulasRepository.Remove(aula);
            bool result = _db.Save();
            if (!result)
                return StatusCode(500, $"Ocurrio un error al borrar las aulas {idAulas}.");
            return new NoContentResult();
        }

        // GET api/Aulas
        [HttpGet]
        public IEnumerable<AulasDetailsDTO> GetAll()
        {
            var aulas = _db.AulasRepository.Get(includeProperties: "TipoAulaMateria");
            return _mapper.Map<IEnumerable<Aulas>, IEnumerable<AulasDetailsDTO>>(aulas);
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
            return new ObjectResult(_mapper.Map<AulasDetailsDTO>(aula));
        }

        // PUT api/Aulas/1
        [HttpPut("{id}")]
        public IActionResult Update(byte id, [FromBody] AulasDTO aula)
        {
            if (!AulaExist(id))
                return NotFound($"No existe el aula {id} a actualizar");
            aula.IdAula = id;
            _db.AulasRepository.Update(_mapper.Map<Aulas>(aula));
            bool result = _db.Save();
            if (!result)
                return StatusCode(500, $"Ocurrio un error al actualizar el aula {aula.IdAula} : {aula.NombreAula}");
            return new NoContentResult();
        }

        private bool AulaExist(byte idAula)
        {
            return _db.AulasRepository.Exists(aula => aula.IdAula == idAula);
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
