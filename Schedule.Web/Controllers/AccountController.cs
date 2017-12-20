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

                //User.AddIdentity();
                using (var httpClient = new HttpClient())
                {
                    var nvc = new List<KeyValuePair<string, string>>();
                    nvc.Add(new KeyValuePair<string, string>("username", model.Username));
                    nvc.Add(new KeyValuePair<string, string>("password", model.Password));

                    var req = new HttpRequestMessage(HttpMethod.Post, _appSettings.Value.URLBaseAPI + "token") { Content = new FormUrlEncodedContent(nvc) };
                    var response = await httpClient.SendAsync(req);
                    //HttpHelpers.InitializeHttpClient(httpClient, _appSettings.Value.URLBaseAPI);
                    var stringContent = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(model), Encoding.UTF8, "application/x-www-form-urlencoded");
                    //httpClient.PostAsync("url")
                    //HttpResponseMessage response = await httpClient.PostAsJsonAsync("token", stringContent);

                    if (response.IsSuccessStatusCode)
                    {
                        //No me cuadra tener esto aca 
                        string secretKey = "mysupersecret_secretkey!123";
                        var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
                        var tokenValidationParameters = new TokenValidationParameters
                        {
                            // The signing key must match!
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = signingKey,

                            // Validate the JWT Issuer (iss) claim
                            ValidateIssuer = true,
                            ValidIssuer = "ExampleIssuer",

                            // Validate the JWT Audience (aud) claim
                            ValidateAudience = true,
                            ValidAudience = "ExampleAudience",

                            // Validate the token expiry
                            ValidateLifetime = true,

                            // If you want to allow a certain amount of clock drift, set that here:
                            ClockSkew = TimeSpan.Zero
                        };
                        var handler = new JwtSecurityTokenHandler();
                        var token = await response.Content.ReadAsAsync<TokenDTO>();
                        SecurityToken xd = null;
                        //var algo = handler.ReadToken(token.AuthenticationToken) as JwtSecurityToken;//.
                        ClaimsPrincipal principal = handler.ValidateToken(token.AuthenticationToken, tokenValidationParameters, out xd);
                        
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                        return RedirectToAction("Index", "Home");
                        //TokenDTO token = await response.Content.ReadAsAsync<TokenDTO>();


                        //Interesante esto de aca
                        var accessToken = await HttpContext.GetTokenAsync("access_token");
                        var client = new HttpClient();
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AuthenticationToken);

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

        public async Task<ActionResult> Logout()
        {
            string token = Request.Cookies["Token"];

            if (!String.IsNullOrEmpty(token))
            {
                using (var httpClient = new HttpClient())
                {
                    HttpHelpers.InitializeHttpClient(httpClient, _appSettings.Value.URLBaseAPI, token);
                    HttpResponseMessage response = await httpClient.DeleteAsync(String.Format("api/Account/Logout/{0}", token));
                }
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
