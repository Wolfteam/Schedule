using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Schedule.Web.Models;
using Schedule.Entities;
using Microsoft.Extensions.Options;
using Schedule.Web.ViewModels;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Schedule.Web.Controllers
{
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

        public async Task<IActionResult> Index()
        {
            List<Privilegios> listaPrivilegios = await GetAllPrivilegiosByToken();

            if (listaPrivilegios == null)
            {
                Response.Cookies.Delete("Token");
                return RedirectToAction("Index", "Account");
            }

            return View(listaPrivilegios);
        }

        private async Task<List<Privilegios>> GetAllPrivilegiosByToken()
        {
            List<Privilegios> listaPrivilegios = null;
            string token = Request.Cookies["Token"];
            if (String.IsNullOrEmpty(token)) return listaPrivilegios;

            HttpResponseMessage response = await _httpClient.GetAsync(String.Format("api/Account/Privilegios/{0}", token));

            if (response.IsSuccessStatusCode)
            {
                listaPrivilegios = await response.Content.ReadAsAsync<List<Privilegios>>();
            }

            return listaPrivilegios;
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
