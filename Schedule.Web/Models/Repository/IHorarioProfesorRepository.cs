using Microsoft.AspNetCore.Mvc;
using Schedule.Entities;
using System.Net.Http;
using System.Threading.Tasks;

namespace Schedule.Web.Models.Repository
{
    public interface IHorarioProfesorRepository
        : IRepository<HorarioProfesorDTO, HorarioProfesorDetailsDTO>
    {
        /// <summary>
        /// Genera y obtiene la planificacion academica
        /// </summary>
        /// <returns>FileContentResult o NoContentResult</returns>
        Task<IActionResult> GeneratePlanificacionAcademica();

        /// <summary>
        /// Genera y obtiene la planificacion de aulas
        /// </summary>
        /// <returns>FileContentResult o NoContentResult</returns>
        Task<IActionResult> GeneratePlanificacionAulas();

        /// <summary>
        /// Genera y obtiene la planificacion de horarios
        /// </summary>
        /// <returns>FileContentResult o NoContentResult</returns>
        Task<IActionResult> GeneratePlanificacionHorario();
    }
}
