using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Schedule.API.Models;
using Schedule.Entities;
using System.Collections.Generic;

namespace Schedule.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = Roles.ADMINISTRADOR)]
    public class PrivilegiosController : BaseController
    {
        public PrivilegiosController(HorariosContext context) 
            : base(context)
        {
        }

        // GET api/Privilegios
        [HttpGet]
        public IEnumerable<PrivilegiosDTO> GetAll()
        {
            var privilegios = _db.PrivilegiosRepository.GetAll();
            return Mapper.Map<IEnumerable<PrivilegiosDTO>>(privilegios);
        }
    }
}