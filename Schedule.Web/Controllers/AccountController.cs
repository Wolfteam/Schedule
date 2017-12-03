using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Schedule.Entities;
using Schedule.Web.Helpers;
using Schedule.Web.ViewModels;
using System;
using System.Net;
using System.Net.Http;
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
                HttpHelpers.InitializeHttpClient(_httpClient, _appSettings.Value.URLBaseAPI);
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
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/Account/Login", new UsuarioDTO()
                {
                    Username = model.Username,
                    Password = model.Password
                });

                if (response.IsSuccessStatusCode)
                {
                    TokenDTO token = await response.Content.ReadAsAsync<TokenDTO>();
                    if (token != null)
                    {
                        double expiricyTime = (token.ExpiricyDate - token.CreateDate).TotalMinutes;
                        CreateCookie("Token", token.AuthenticationToken, expiricyTime);
                        CreateCookie("User", model.Username, expiricyTime);
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

        /// <summary>  
        /// Crea una cookie  
        /// </summary>  
        /// <param name="key">Identificador unico</param>  
        /// <param name="value">Valor a guardar</param>  
        /// <param name="expireTime">Tiempo de expiracion en minutos</param>  
        private void CreateCookie(string key, string value, double expireTime = 20)
        {
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddMinutes(expireTime);
            Response.Cookies.Append(key, value, option);
        }
    }
}
