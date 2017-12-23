using Schedule.Entities;
using System.Net.Http;

namespace Schedule.Web.Models.Repository
{
    public class ProfesorRepository : Repository<ProfesorDTO, ProfesorDetailsDTO>, IProfesorRepository
    {
        //private readonly HttpClient _httpClient;
        //private const string _apiProfesor = "api/Profesor";

        public ProfesorRepository(HttpClient httpClient, string urlEntityApi)
            : base(httpClient, urlEntityApi)
        {
        }

        // public ProfesorRepository(HttpClient httpClient)
        // {
        //     //_httpClient = httpClient;
        // }

        // public async Task<bool> Add(ProfesorDTO profesor)
        // {
        //     HttpResponseMessage response = await _httpClient.PostAsJsonAsync(_apiProfesor, profesor);
        //     if (response.IsSuccessStatusCode)
        //         return true;
        //     else
        //         return false;
        // }

        // /// <summary>
        // /// Obtiene una lista con todos los profesores
        // /// </summary>
        // /// <returns>Lista de tipo ProfesorDetailsDTO</returns>
        // public async Task<List<ProfesorDetailsDTO>> GetAll()
        // {
        //     List<ProfesorDetailsDTO> profesores = new List<ProfesorDetailsDTO>();
        //     HttpResponseMessage response = await _httpClient.GetAsync(_apiProfesor);
        //     if (response.IsSuccessStatusCode)
        //     {
        //         profesores = await response.Content.ReadAsAsync<List<ProfesorDetailsDTO>>();
        //     }
        //     return profesores;
        // }

        // /// <summary>
        // /// Obtiene un profesor en particular
        // /// </summary>
        // /// <param name="cedula">Cedula del profesor</param>
        // /// <returns>Objeto de tipo ProfesorDetailsDTO</returns>
        // public async Task<ProfesorDetailsDTO> Get(int cedula)
        // {
        //     ProfesorDetailsDTO profesor = null;
        //     HttpResponseMessage response = await _httpClient.GetAsync($"{_apiProfesor}/{cedula}");
        //     if (response.IsSuccessStatusCode)
        //     {
        //         profesor = await response.Content.ReadAsAsync<ProfesorDetailsDTO>();
        //     }
        //     return profesor;
        // }

        // public async Task<bool> Remove(int cedula)
        // {
        //     HttpResponseMessage response = await _httpClient.DeleteAsync($"{_apiProfesor}/{cedula}");
        //     if (response.IsSuccessStatusCode)
        //         return true;
        //     else
        //         return false;
        // }

        // public async Task<bool> Update(int cedula, ProfesorDTO profesor)
        // {
        //     HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"{_apiProfesor}/{cedula}", profesor);
        //     if (response.IsSuccessStatusCode)
        //         return true;
        //     else
        //         return false;
        // }
    }
}