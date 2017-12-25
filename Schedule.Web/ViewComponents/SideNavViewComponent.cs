using Microsoft.AspNetCore.Mvc;
using Schedule.Entities;
using Schedule.Web.ViewModels;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Schedule.Web.ViewComponents
{
    /// <summary>
    /// Esta componente permite renderizar un menu del SideNav
    /// acorde a si es Administrador o no
    /// </summary>
    public class SideNavViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            //TODO: Deberia traer todos los privilegios posibles de la api?
            List<Privilegios> privilegios = new List<Privilegios>();
            string[] allPrivilegios = Enum.GetNames(typeof(Privilegios));
            var cp = UserClaimsPrincipal;
            foreach (var claim in cp.Claims)
            {
                //Si el claim es de tipo role y el valor equivale a alguno de los de allPrivilegios
                if (allPrivilegios.Any(c => claim.Type == ClaimTypes.Role && c == claim.Value))
                {
                    privilegios.Add((Privilegios)Enum.Parse(typeof(Privilegios), claim.Value));
                }
            }

            string fullname = $"{cp.FindFirstValue(ClaimTypes.GivenName)} {cp.FindFirstValue(ClaimTypes.Surname)}";

            var model = new SideNavViewModel
            {
                FullName = fullname,
                IPAdress = "npi",
                Privilegios = privilegios,
                Username = User.Identity.Name
            };
            return View(model);
        }
    }
}