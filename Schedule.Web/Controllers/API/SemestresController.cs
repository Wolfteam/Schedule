using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Schedule.Entities;
using Schedule.Web.Controllers;
using Schedule.Web.Models;
using Schedule.Web.Models.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Schedule.Web.Controllers.API
{
    [Route("api/[controller]")]
    [Authorize(Roles = Roles.ADMINISTRADOR)]
    public class SemestresController : BaseController
    {
        private readonly UnitOfWork _unitOfWork;
        public SemestresController(IOptions<AppSettings> appSettings, IHttpClientsFactory httpClientsFactory)
            : base(appSettings, httpClientsFactory)
        {
            _unitOfWork = new UnitOfWork(httpClientsFactory);
        }

        [HttpGet]
        public async Task<IEnumerable<SemestreDTO>> GetAllAsync()
        {
            _unitOfWork.Token = await HttpContext.GetTokenAsync(_tokenName);
            return await _unitOfWork.SemestresRepository.GetAllAsync();
        }
    }
}