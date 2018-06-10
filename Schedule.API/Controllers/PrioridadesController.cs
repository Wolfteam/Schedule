using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Schedule.Entities;
using System.Collections.Generic;
using Schedule.API.Models;

namespace Schedule.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = Roles.ADMINISTRADOR)]
    public class PrioridadesController : BaseController
    {
        public PrioridadesController(HorariosContext context) 
            : base(context)
        {
        }

        // GET api/Prioridades
        [HttpGet]
        public IEnumerable<PrioridadProfesorDTO> GetAll()
        {
            var prioridades = _db.PrioridadesRepository.GetAll();
            return Mapper.Map<IEnumerable<PrioridadProfesorDTO>>(prioridades);
        }
    }
}