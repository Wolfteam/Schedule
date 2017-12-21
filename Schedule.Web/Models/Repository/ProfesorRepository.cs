using Schedule.Entities;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Schedule.Web.Models.Repository
{
    public class ProfesorRepository
    {
        private readonly HttpClient _httpClient = null;

        public ProfesorRepository(HttpClient httpClient, string token)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (token != null)
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        /// <summary>
        /// Obtiene una lista con todos los profesores
        /// </summary>
        /// <returns>Lista de tipo ProfesorDetailsDTO</returns>
        public async Task<List<ProfesorDetailsDTO>> GetAll()
        {
            List<ProfesorDetailsDTO> profesores = new List<ProfesorDetailsDTO>();
            HttpResponseMessage response = await _httpClient.GetAsync("api/Profesor");
            if (response.IsSuccessStatusCode)
            {
                profesores = await response.Content.ReadAsAsync<List<ProfesorDetailsDTO>>();
            }
            return profesores;
        }

        /// <summary>
        /// Obtiene un profesor en particular
        /// </summary>
        /// <param name="cedula">Cedula del profesor</param>
        /// <returns>Objeto de tipo ProfesorDetailsDTO</returns>
        public async Task<ProfesorDetailsDTO> Get(int cedula)
        {
            ProfesorDetailsDTO profesor = null;
            HttpResponseMessage response = await _httpClient.GetAsync("api/Profesores/" + cedula);
            if (response.IsSuccessStatusCode)
            {
                profesor = await response.Content.ReadAsAsync<ProfesorDetailsDTO>();
            }
            return profesor;
        }
    }
}