using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Schedule.API.Filters
{
    public class PermisoAttribute : ActionFilterAttribute
    {
        //public RolesPermisos Permiso { get; set; }

        //public override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    base.OnActionExecuting(filterContext);

        //    if (!FrontUser.TienePermiso(this.Permiso))
        //    {
        //        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
        //        {
        //            controller = "Home",
        //            action = "Index"
        //        }));
        //    }
        //}

    }
}
