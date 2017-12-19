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
    public class CarrerasController : BaseController
    {
        // GET api/Carreras
        [HttpGet]
        public IEnumerable<CarreraDTO> GetAll()
        {
            var carreras =  _db.CarrerasRepository.GetAll();
            return Mapper.Map<IEnumerable<CarreraDTO>>(carreras);
        }
    }
}
