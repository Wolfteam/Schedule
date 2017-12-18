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
    public class CarrerasController : Controller
    {
        private readonly UnitOfWork _db = new UnitOfWork();

        // GET api/Carreras
        [HttpGet]
        public IEnumerable<CarreraDTO> GetAll()
        {
            var carreras =  _db.CarrerasRepository.GetAll();
            return Mapper.Map<IEnumerable<CarreraDTO>>(carreras);
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}
