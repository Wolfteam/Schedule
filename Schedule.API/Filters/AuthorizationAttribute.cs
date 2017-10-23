using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Schedule.API.Models.Repositories;
using Schedule.Entities;
using System.Collections.Generic;

namespace Schedule.API.Filters
{
    public class AuthorizationAttribute : ActionFilterAttribute
    {
        private readonly TokenRepository _tokenService = new TokenRepository();
        private readonly Privilegios _privilegio;

        public AuthorizationAttribute(Privilegios privilegio)
        {
            _privilegio = privilegio;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string token = context.ActionArguments["Token"] as string;

            Privilegios privilegio = _tokenService.GetAllPrivilegiosByToken(token);

            if (privilegio != _privilegio)
            {
                context.HttpContext.Response.StatusCode = 401;
                context.Result = new ContentResult()
                {
                    Content = "El token expiro, no contiene ningun privilegio asociado o no tiene permiso para acceder al recurso"
                };
                return;
            }
            base.OnActionExecuting(context);
        }
    }
}
