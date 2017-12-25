using Microsoft.AspNetCore.Mvc;
using Schedule.Entities;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Schedule.Web.Models.Repository
{
    public class HorarioProfesorRepository
        : Repository<HorarioProfesorDTO, HorarioProfesorDetailsDTO>, IHorarioProfesorRepository
    {
        public HorarioProfesorRepository(IHttpClientsFactory httpClientsFactory, string urlEntityApi)
            : base(httpClientsFactory, urlEntityApi)
        {
        }

        public async Task<IActionResult> GeneratePlanificacionAcademica()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{_urlEntityApi}/PlanificacionAcademica");
            return await ReadPlanificacionResponse(response);
        }

        public async Task<IActionResult> GeneratePlanificacionAulas()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{_urlEntityApi}/PlanificacionAulas");
            return await ReadPlanificacionResponse(response);
        }

        public async Task<IActionResult> GeneratePlanificacionHorario()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{_urlEntityApi}/PlanificacionHorario");
            return await ReadPlanificacionResponse(response);
        }

        /// <summary>
        /// Metodo helper que lee la respuesta a un FileContentResult
        /// </summary>
        /// <param name="response">Response de la peticion</param>
        /// <returns>FileContentResult o NoContentResult</returns>
        private async Task<IActionResult> ReadPlanificacionResponse(HttpResponseMessage response)
        {
            var mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            byte[] fileBytes;
            if (!response.IsSuccessStatusCode)
                return new NoContentResult();
            fileBytes = await response.Content.ReadAsByteArrayAsync();
            return new FileContentResult(fileBytes, mimeType);
        }
    }
}