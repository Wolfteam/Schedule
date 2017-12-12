using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Schedule.Entities;
using Schedule.Web.Models.Repository;
using Schedule.Web.Filters;
using Microsoft.Extensions.Options;
using Schedule.Web.Helpers;

namespace Schedule.Web.Controllers
{
    [AuthenticateAttribute]
    public class CargarDisponibilidadController : BaseController
    {
        #region Constructor
        public CargarDisponibilidadController(IOptions<AppSettings> appSettings)
            : base(appSettings)
        {

        }
        #endregion

        public async Task<ActionResult> Index()
        {
            string token = Request.Cookies["Token"];
            ProfesorRepository pr = new ProfesorRepository(_httpClient);

            var privilegios = await HttpHelpers.GetAllPrivilegiosByToken(_httpClient, token);

            List<ProfesorDetailsDTO> model = new List<ProfesorDetailsDTO>();
            if (privilegios.Contains(Privilegios.Administrador))
                model = await pr.GetAll();
            else
            {
                TokenRepository tr = new TokenRepository(_httpClient);
                ProfesorDetailsDTO profesor = await tr.GetProfesorInfoByToken(token);
                model.Add(profesor);
            }
            return View(model.OrderBy(x => x.Nombre).ToList());
        }
    }
}
