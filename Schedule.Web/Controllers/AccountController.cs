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
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace Schedule.Web.Controllers
{
    public class AccountController : Controller
    {
        IOptions<AppSettings> _appSettings;
        #region Constructor
        public AccountController(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings;
        }
        #endregion

        [AllowAnonymous]
        /// <summary>
        /// Este metodo devuelve la vista de login o redirecciona a home
        /// en caso de estar autenticado
        /// </summary>
        /// <param name="returnUrl">Es llenado cuando tratas de acceder a un sitio 
        /// authorized sin estar autenticado
        /// </param>
        /// <returns>IActionResult</returns>
        public IActionResult Index(string returnUrl = null)
        {
            //se pasa a la ViewData["ReturnUrl"] ya que en el form sera renderizado
            //si returnUrl es null no muestra nada pero sino si coloca algo
            ViewData["ReturnUrl"] = returnUrl;
            if (!User.Identity.IsAuthenticated)
            {
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    var nvc = new List<KeyValuePair<string, string>>();
                    nvc.Add(new KeyValuePair<string, string>("username", model.Username));
                    nvc.Add(new KeyValuePair<string, string>("password", model.Password));

                    var req = new HttpRequestMessage(HttpMethod.Post, _appSettings.Value.URLBaseAPI + "token") { Content = new FormUrlEncodedContent(nvc) };
                    var response = await httpClient.SendAsync(req);

                    if (response.IsSuccessStatusCode)
                    {
                        string secretKey = "mysupersecret_secretkey!123";
                        var tokenValidationParameters = TokenParameters.GetTokenValidationParameters(secretKey);

                        var handler = new JwtSecurityTokenHandler();
                        var token = await response.Content.ReadAsAsync<TokenDTO>();
                        SecurityToken validatedToken = null;
                        ClaimsPrincipal principal = handler.ValidateToken(token.AuthenticationToken, tokenValidationParameters, out validatedToken);

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                        //returnUrl es pasado automaticamente si es que hay algo en esa variable
                        if (String.IsNullOrEmpty(returnUrl))
                            return RedirectToAction("Index", "Home");
                        else
                            return Redirect(returnUrl);

                        //Interesante esto de aca
                        // var accessToken = await HttpContext.GetTokenAsync("access_token");
                        // var client = new HttpClient();
                        // httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AuthenticationToken);
                    }
                    else if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        ModelState.AddModelError("", "Fallo la conexion a la API");
                        return View("Index", model);
                    }
                    ModelState.AddModelError("", "Usuario o clave invalidas");
                    return View("Index", model);
                }
            }
            ModelState.AddModelError("", "El modelo no es valido");
            return View("Index", model);
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
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
