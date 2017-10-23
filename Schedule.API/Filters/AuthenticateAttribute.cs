using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Schedule.API.Models.Repositories;

namespace Schedule.API.Filters
{
    /// <summary>
    /// Permite el acceso a la api y sus metodos
    /// siempre y cuando el token asociado sea valido
    /// </summary>
    public class AuthenticateAttribute : ActionFilterAttribute
    {
        private readonly TokenRepository _tokenService = new TokenRepository();
        public override void OnActionExecuting(ActionExecutingContext context)
        {           
            try
            {
                string token = context.ActionArguments["Token"] as string;
                if (!_tokenService.Validate(token))
                {
                    context.HttpContext.Response.StatusCode = 401;
                    context.Result = new ContentResult()
                    {
                        Content = "El token no existe o expiro"
                    };
                    return;
                }
            }
            catch (System.Exception)
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
