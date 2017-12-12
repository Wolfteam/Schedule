using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Schedule.Entities;
using Schedule.Web.Helpers;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Schedule.Web.Controllers
{
    public class BaseController : Controller
    {
        protected string _token;
        protected readonly IOptions<AppSettings> _appSettings;
        protected static HttpClient _httpClient;
        protected HttpMessageHandler _handler;
        public BaseController(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings;
        }

        /// <summary>
        /// Este metodo se ejecuta despues del constructor, notese que siempre se crea
        /// el cliente http con el token, esto es debido a que para entrar en este punto
        /// debe tener un token valido
        /// </summary>
        /// <param name="context">Contexto</param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            _token = HttpContext.Request.Cookies["Token"];
            if (_httpClient != null)
            {
                _httpClient.DefaultRequestHeaders.Remove("Token");
                _httpClient.DefaultRequestHeaders.Add("Token", _token);
            }
            
            if (_httpClient == null)
            {
                _httpClient = new HttpClient();
                HttpHelpers.InitializeHttpClient(_httpClient, _appSettings.Value.URLBaseAPI, _token);
            }
        }
    }
}