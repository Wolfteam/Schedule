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
    public class PrioridadesController : BaseController
    {
        // GET api/Prioridades
        [HttpGet]
        public IEnumerable<PrioridadProfesorDTO> GetAll()
        {
            var prioridades = _db.PrioridadesRepository.GetAll();
            return Mapper.Map<IEnumerable<PrioridadProfesorDTO>>(prioridades);
        }
    }
}