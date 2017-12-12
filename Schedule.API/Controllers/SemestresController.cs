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
    public class SemestresController : Controller
    {
        private readonly SemestresRepository _db = new SemestresRepository();

        // GET api/Semestres
        [HttpGet]
        public IEnumerable<SemestreDTO> GetAll()
        {
            return _db.Get();
        }
    }
}
