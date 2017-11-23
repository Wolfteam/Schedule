using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Schedule.Entities;
using Schedule.Web.Models.Repository;
using Schedule.Web.Filters;
using Microsoft.Extensions.Options;
using Schedule.Web.ViewModels;
using Schedule.Web.Helpers;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Schedule.Web.Controllers
{
    [AuthenticateAttribute]
    public class CargarDisponibilidadController : Controller
    {
        private readonly IOptions<AppSettings> _appSettings;
        static HttpClient _httpClient = null;

        public CargarDisponibilidadController(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings;
            if (_httpClient == null)
            {
                _httpClient = new HttpClient();
                HttpHelpers.InitializeHttpClient(_httpClient, _appSettings.Value.URLBaseAPI);
            }
        }

        public async Task<ActionResult> Index()
        {
            string token = Request.Cookies["Token"];

            ProfesorRepository pr = new ProfesorRepository(_httpClient);

            var privilegios = await HttpHelpers.GetAllPrivilegiosByToken(_httpClient, token);

            List<ProfesorDetailsDTO> model = new List<ProfesorDetailsDTO>();
            if (privilegios.Contains(Privilegios.Administrador))
                model = await pr.GetAll();
            else
            {
                TokenRepository tr = new TokenRepository(_httpClient);
                ProfesorDetailsDTO profesor = await tr.GetProfesorInfoByToken(token);
                model.Add(profesor);
            }
            return View(model.OrderBy(x => x.Nombre).ToList());
        }
    }
}
