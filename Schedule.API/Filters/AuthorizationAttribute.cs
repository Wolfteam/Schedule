using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Schedule.BLL;
using Schedule.Entities;
using System.Collections.Generic;

namespace Schedule.API.Filters
{
    public class AuthorizationAttribute : ActionFilterAttribute
    {
        private TokenServices _tokenService;
        private readonly Privilegios _privilegio;

        public AuthorizationAttribute(Privilegios privilegio)
        {
            _privilegio = privilegio;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _tokenService = new TokenServices();
            Token token = context.ActionArguments["Token"] as Token;

            List<Privilegios> listaPrivilegios = _tokenService.GetAllPrivilegiosByToken(token.AuthenticationToken);

            if (listaPrivilegios == null || !listaPrivilegios.Contains(_privilegio))
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
