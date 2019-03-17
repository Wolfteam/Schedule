using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Schedule.Entities;
using System.Collections.Generic;
using Schedule.API.Models;
using Schedule.API.Models.Repositories;

namespace Schedule.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = Roles.ADMINISTRADOR)]
    public class PrioridadesController : BaseController
    {
        public PrioridadesController(IUnitOfWork uow, IMapper mapper) 
            : base(uow, mapper)
        {
        }

        // GET api/Prioridades
        [HttpGet]
        public IEnumerable<PrioridadProfesorDTO> GetAll()
        {
            var prioridades = _db.PrioridadesRepository.GetAll();
            return _mapper.Map<IEnumerable<PrioridadProfesorDTO>>(prioridades);
        }
    }
}