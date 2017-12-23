using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Schedule.Entities;
using Schedule.Web.Helpers;
using System.Collections.Generic;
using System.Net.Http;

namespace Schedule.Web.Filters
{
    /// <summary>
    /// No esta en uso. Queda como modelo por si lo llego a usar
    /// </summary>
    public class AuthorizationAttribute //: ActionFilterAttribute
    {
        
        // private readonly Privilegios _privilegio;
        // static HttpClient _httpClient = null;
        // IOptions<AppSettings> _appSettings;

        public AuthorizationAttribute(Privilegios privilegio, IOptions<AppSettings> appSettings)
        {
            // if (_httpClient == null)
            // {
            //     _httpClient = new HttpClient();
            //     HttpHelpers.InitializeHttpClient(_httpClient, _appSettings.Value.URLBaseAPI);
            // }
            // _privilegio = privilegio;
        }

        // public override async void OnActionExecuting(ActionExecutingContext context)
        // {
        //     string token = context.ActionArguments["Token"] as string;

        //     List<Privilegios> privilegios = await HttpHelpers.GetAllPrivilegiosByToken(_httpClient, token);

        //     if (!privilegios.Contains(_privilegio))
        //     {
        //         context.HttpContext.Response.Cookies.Delete("Token");
        //         context.Result = new RedirectToRouteResult(
        //             new RouteValueDictionary(new
        //             {
        //                 controller = "Home",
        //                 action = "Index"
        //             })
        //         );
        //         return;
        //     }
        //     base.OnActionExecuting(context);
        // }
    }
}
