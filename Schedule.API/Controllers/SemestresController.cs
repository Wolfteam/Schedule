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
    public class SemestresController : Controller
    {
        private readonly UnitOfWork _db = new UnitOfWork();

        // GET api/Semestres
        [HttpGet]
        public IEnumerable<SemestreDTO> GetAll()
        {
            var semestres = _db.SemestresRepository.GetAll();
            return Mapper.Map<IEnumerable<SemestreDTO>>(semestres);
        }
        
        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}