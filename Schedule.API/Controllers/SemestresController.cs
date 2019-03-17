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
    public class SemestresController : BaseController
    {
        public SemestresController(IUnitOfWork uow, IMapper mapper) 
            : base(uow, mapper)
        {
        }

        // GET api/Semestres
        [HttpGet]
        public IEnumerable<SemestreDTO> GetAll()
        {
            var semestres = _db.SemestresRepository.GetAll();
            return _mapper.Map<IEnumerable<SemestreDTO>>(semestres);
        }
    }
}