using Schedule.Entities;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Schedule.Web.Models.Repository
{
    public class DisponibilidadProfesorRepository
    {
        private static HttpClient _httpClient;
        private const string apiDisponibilidad = "api/Disponibilidad";
        public DisponibilidadProfesorRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Obtiene la disponibilidad de un profesor en particular
        /// </summary>
        /// <param name="cedula">Cedula del profesor</param>
        /// <returns>DisponibilidadProfesorDetailsDTO</returns>
        public async Task<DisponibilidadProfesorDetailsDTO> GetByCedula(int cedula)
        {
            var disponibilidad = new DisponibilidadProfesorDetailsDTO();
            HttpResponseMessage response = await _httpClient.GetAsync($"{apiDisponibilidad}/{cedula}");
            if (response.IsSuccessStatusCode)
                disponibilidad = await response.Content.ReadAsAsync<DisponibilidadProfesorDetailsDTO>();
            return disponibilidad;
        }
    }
}