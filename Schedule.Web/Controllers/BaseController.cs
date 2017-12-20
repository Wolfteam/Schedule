using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Schedule.Entities;
using System.Net.Http;

namespace Schedule.Web.Controllers
{
    public class BaseController : Controller
    {
        protected readonly IOptions<AppSettings> _appSettings;
        protected static HttpClient _httpClient;
        public BaseController(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings;
        }
    }
}