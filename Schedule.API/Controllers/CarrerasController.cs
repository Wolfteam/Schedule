using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Schedule.API.Models.Repositories;
using Schedule.Entities;
using System.Collections.Generic;

namespace Schedule.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = Roles.ADMINISTRADOR)]
    public class CarrerasController : BaseController
    {
        public CarrerasController(IUnitOfWork uow, IMapper mapper)
            : base(uow, mapper)
        {
        }

        // GET api/Carreras
        [HttpGet]
        public IEnumerable<CarreraDTO> GetAll()
        {
            var carreras = _db.CarrerasRepository.GetAll();
            return _mapper.Map<IEnumerable<CarreraDTO>>(carreras);
        }
    }
}
