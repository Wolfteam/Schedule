using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Schedule.Web.Models;
using Schedule.Entities;
using Microsoft.Extensions.Options;
using System.Net.Http;
using Schedule.Web.Filters;
using Schedule.Web.Helpers;

namespace Schedule.Web.Controllers
{
    [AuthenticateAttribute]
    public class HomeController : Controller
    {
        #region Variables   
        IOptions<AppSettings> _appSettings;
        static HttpClient _httpClient = null;
        #endregion

        #region Constructor
        public HomeController(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings;
            if (_httpClient == null)
            {
                _httpClient = new HttpClient();
                HttpHelpers.InitializeHttpClient(_httpClient, _appSettings.Value.URLBaseAPI);
            }
        }
        #endregion

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
