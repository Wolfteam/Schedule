using Microsoft.AspNetCore.Mvc;
using Schedule.Data;
using Schedule.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Schedule.Web.Controllers
{
    public class ProfesorController : Controller
    {
        private IProfesor _assets;
        public ProfesorController(IProfesor assets)
        {
            _assets = assets;
        }

        public IActionResult Index()
        {
            var assetsModels = _assets.GetAll();
            var listaProfesores = assetsModels.Select(result => new ProfesorViewModel
            {
                Cedula = result.Cedula,
                Nombre = result.Nombre,
                Apellido = result.Apellido
            });

            var model = new ProfesorListingViewModel
            {
                Profesores = listaProfesores
            };

            return View(model);
        }
    }
}
