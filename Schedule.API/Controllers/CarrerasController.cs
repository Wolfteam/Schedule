using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Schedule.Entities;
using System.Collections.Generic;
using Schedule.API.Models;

namespace Schedule.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrador")]
    public class CarrerasController : BaseController
    {
        public CarrerasController(HorariosContext context) 
            : base(context)
        {
        }

        // GET api/Carreras
        [HttpGet]
        public IEnumerable<CarreraDTO> GetAll()
        {
            var carreras = _db.CarrerasRepository.GetAll();
            return Mapper.Map<IEnumerable<CarreraDTO>>(carreras);
        }
    }
}
