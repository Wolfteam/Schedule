using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Schedule.Entities;
using Schedule.Web.ViewModels;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Schedule.Web.Controllers
{
    public class AccountController : Controller
    {
        #region Variables
        IOptions<AppSettings> _appSettings;
        static HttpClient _httpClient = null;
        #endregion

        #region Constructor
        public AccountController(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings;
            if (_httpClient == null)
            {
                _httpClient = new HttpClient();
                InitializeHttpClient(_appSettings.Value.URLBaseAPI);
            }
        }
        #endregion

        public IActionResult Index()
        {
            string token = Request.Cookies["Token"];
            if (String.IsNullOrEmpty(token))
            {
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/Account/Login", new Usuario()
                {
                    Username = model.Username,
                    Password = model.Password
                });

                if (response.IsSuccessStatusCode)
                {
                    Token token = await response.Content.ReadAsAsync<Token>();
                    if (token != null)
                    {
                        SaveToken(token);
                        return RedirectToAction("Index", "Home");
                    }
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    ModelState.AddModelError("", "Fallo la conexion a la API");
                    return View("Index", model);
                }
                ModelState.AddModelError("", "Usuario o clave invalidas");
                return View("Index", model);
            }
            ModelState.AddModelError("", "El modelo no es valido");
            return View("Index", model);
        }

        public async Task<ActionResult> Logout()
        {
            string token = Request.Cookies["Token"];

            if (!String.IsNullOrEmpty(token))
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync(String.Format("api/Account/Logout/{0}", token));

                Response.Cookies.Delete("Token");
            }
            return RedirectToAction("Index", "Account");
        }

        static void InitializeHttpClient(string urlBaseAPI)
        {
            _httpClient.BaseAddress = new Uri(urlBaseAPI);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private void SaveToken(Token token)
        {
            CookieOptions cookiesOptions = new CookieOptions
            {
                Expires = token.ExpiricyDate
            };
            Response.Cookies.Delete("Token");
            Response.Cookies.Append("Token", token.AuthenticationToken, cookiesOptions);
        }
    }
}
