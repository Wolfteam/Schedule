using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Schedule.API.Filters;
using Schedule.API.Models.Repositories;
using Schedule.Entities;
using System.Collections.Generic;

namespace Schedule.API.Controllers
{
    [Route("api/[controller]")]
    [GlobalAttibute]
    [AuthenticateAttribute]
    [AuthorizationAttribute(Entities.Privilegios.Administrador)]
    public class PrioridadesController : Controller
    {
        private readonly UnitOfWork _db = new UnitOfWork();

        // GET api/Prioridades
        [HttpGet]
        public IEnumerable<PrioridadProfesorDTO> GetAll()
        {
            var prioridades = _db.PrioridadesRepository.GetAll();
            return Mapper.Map<IEnumerable<PrioridadProfesorDTO>>(prioridades);
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}