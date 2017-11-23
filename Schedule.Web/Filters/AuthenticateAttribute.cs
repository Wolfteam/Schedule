using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;

namespace Schedule.Web.Filters
{
    /// <summary>
    /// Se asume que si tiene un token es porque esta autenticado
    /// </summary>
    public class AuthenticateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string token = context.HttpContext.Request.Cookies["Token"];
            if (String.IsNullOrEmpty(token))
            {
                context.HttpContext.Response.Cookies.Delete("Token");
                context.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new
                    {
                        controller = "Account",
                        action = "Index"
                    })
                );
                return;
            }
            base.OnActionExecuting(context);
        }
    }
}
