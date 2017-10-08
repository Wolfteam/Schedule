using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
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
            try
            {
                string token = context.HttpContext.Request.Cookies["Token"];
                if (String.IsNullOrEmpty(token))
                {
                    //context.Result = new RedirectToRouteResult(
                    //    new RouteValueDictionary(new { controller = "Account", action = "Index" })
                    //);
                    context.HttpContext.Response.StatusCode = 400;
                    context.Result = new ContentResult()
                    {
                        Content = "El cuerpo no es de tipo Token"
                    };
                    return;
                }
            }
            catch (Exception)
            {
                context.HttpContext.Response.StatusCode = 400;
                context.Result = new ContentResult()
                {
                    Content = "El cuerpo no es de tipo Token"
                };
                return;
            }
            base.OnActionExecuting(context);
        }
    }
}
