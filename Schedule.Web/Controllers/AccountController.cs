using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Schedule.Entities;
using Schedule.Web.Helpers;
using Schedule.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Schedule.Web.Controllers
{
    public class AccountController : BaseController
    {
        public AccountController(IOptions<AppSettings> appSettings, IHttpClientsFactory httpClientsFactory)
            : base(appSettings, httpClientsFactory)
        {
        }

        /// <summary>
        /// Este metodo devuelve la vista de login o redirecciona a home
        /// en caso de estar autenticado
        /// </summary>
        /// <param name="returnUrl">Es llenado cuando tratas de acceder a un sitio 
        /// authorized sin estar autenticado
        /// </param>
        /// <returns>IActionResult</returns>
        [AllowAnonymous]
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

        [Authorize]
        public IActionResult Forbidden(string returnUrl = null)
        {
             ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    var nvc = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("username", model.Username),
                        new KeyValuePair<string, string>("password", model.Password)
                    };
                    //token es la ruta a donde ir a pedir token( e.g:localhost:5050/token)
                    var req = new HttpRequestMessage(HttpMethod.Post, _appSettings.Value.URLBaseAPI + "token") { Content = new FormUrlEncodedContent(nvc) };
                    var response = await httpClient.SendAsync(req);

                    if (response.IsSuccessStatusCode)
                    {
                        TokenDTO token = await response.Content.ReadAsAsync<TokenDTO>();

                        var handler = new JwtSecurityTokenHandler();
                        var tokenValidationParameters = TokenHelper.GetTokenValidationParameters(_appSettings.Value.SecretKey);
                        ClaimsPrincipal principal = handler.ValidateToken(token.AuthenticationToken, tokenValidationParameters, out SecurityToken validatedToken);                       
                        
                        var tokenAuthProperties = TokenHelper.GetTokenAuthProperties(token);
                        await HttpContext.SignInAsync(principal, tokenAuthProperties);

                        //returnUrl es pasado automaticamente si es que hay algo en esa variable
                        if (String.IsNullOrEmpty(returnUrl))
                            return RedirectToAction("Index", "Home");
                        else
                            return Redirect(returnUrl);
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
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
