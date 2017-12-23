using Schedule.Entities;
using System.Net.Http;

namespace Schedule.Web.Models.Repository
{
    public class PrioridadesRepository : Repository<PrioridadProfesorDTO, PrioridadProfesorDTO>, IPrioridadesRepository
    {
        // private readonly HttpClient _httpClient;
        // private const string _apiPrioridades = "api/Prioridades";

        // public PrioridadesRepository(HttpClient httpClient)
        // {
        //     _httpClient = httpClient;
        // }

        // /// <summary>
        // /// Obtiene todas las prioridades que un profesor puede tener
        // /// </summary>
        // /// <returns>IEnumerable de PrioridadProfesorDTO</returns>
        // public async Task<IEnumerable<PrioridadProfesorDTO>> GetAll()
        // {
        //     List<PrioridadProfesorDTO> prioridades = new List<PrioridadProfesorDTO>();
        //     HttpResponseMessage response = await _httpClient.GetAsync(_apiPrioridades);
        //     if (response.IsSuccessStatusCode)
        //         prioridades = await response.Content.ReadAsAsync<List<PrioridadProfesorDTO>>();
        //     return prioridades;
        // }
        public PrioridadesRepository(HttpClient httpClient, string urlEntityApi) 
            : base(httpClient, urlEntityApi)
        {
        }
    }
}