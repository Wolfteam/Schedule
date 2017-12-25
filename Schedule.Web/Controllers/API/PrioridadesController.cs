using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Schedule.Entities;
using Schedule.Web.Controllers;
using Schedule.Web.Models.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;
using Schedule.Web.Models;

namespace Schedule.Web.Controllers.API
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrador")]
    public class PrioridadesController : BaseController
    {
        private UnitOfWork _unitOfWork;
        public PrioridadesController(IOptions<AppSettings> appSettings, IHttpClientsFactory httpClientsFactory)
            : base(appSettings, httpClientsFactory)
        {
            _unitOfWork = new UnitOfWork(httpClientsFactory);
        }

        [HttpGet]
        public async Task<IEnumerable<PrioridadProfesorDTO>> GetAll()
        {
            _unitOfWork.Token = await HttpContext.GetTokenAsync(_tokenName);
            return await _unitOfWork.PrioridadesRepository.GetAllAsync();
        }
    }
}