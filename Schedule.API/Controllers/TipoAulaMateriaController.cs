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
    public class TipoAulaMateriaController : BaseController
    {
        public TipoAulaMateriaController(HorariosContext context) 
            : base(context)
        {
        }

        // GET api/TipoAulaMateria
        [HttpGet]
        public IEnumerable<TipoAulaMateriaDTO> GetAll()
        {
            var tipos = _db.TipoAulaMateriaRepository.GetAll();
            return Mapper.Map<IEnumerable<TipoAulaMateriaDTO>>(tipos);
        }
    }
}