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
    public class SemestresController : BaseController
    {
        public SemestresController(HorariosContext context) 
            : base(context)
        {
        }

        // GET api/Semestres
        [HttpGet]
        public IEnumerable<SemestreDTO> GetAll()
        {
            var semestres = _db.SemestresRepository.GetAll();
            return Mapper.Map<IEnumerable<SemestreDTO>>(semestres);
        }
    }
}