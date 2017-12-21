using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Schedule.Entities;
using System.Net.Http;

namespace Schedule.Web.Controllers
{
    public class BaseController : Controller
    {
        protected readonly IOptions<AppSettings> _appSettings;
        protected readonly IHttpClientsFactory _httpClientsFactory;
        public BaseController(IOptions<AppSettings> appSettings, IHttpClientsFactory httpClientsFactory)
        {
            _appSettings = appSettings;
            _httpClientsFactory = httpClientsFactory;
        }
    }
}