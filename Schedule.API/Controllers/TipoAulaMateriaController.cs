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
    public class TipoAulaMateriaController : BaseController
    {
        public TipoAulaMateriaController(IUnitOfWork uow, IMapper mapper)
            : base(uow, mapper)
        {
        }

        // GET api/TipoAulaMateria
        [HttpGet]
        public IEnumerable<TipoAulaMateriaDTO> GetAll()
        {
            var tipos = _db.TipoAulaMateriaRepository.GetAll();
            return _mapper.Map<IEnumerable<TipoAulaMateriaDTO>>(tipos);
        }
    }
}