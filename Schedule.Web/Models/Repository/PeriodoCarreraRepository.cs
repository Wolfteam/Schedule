using Schedule.Entities;
using System.Net.Http;
using System.Threading.Tasks;

namespace Schedule.Web.Models.Repository
{
    public class PeriodoCarreraRepository
        : Repository<PeriodoCarreraDTO, PeriodoCarreraDTO>, IPeriodoCarreraRepository
    {
        public PeriodoCarreraRepository(IHttpClientsFactory httpClientsFactory, string urlEntityApi)
            : base(httpClientsFactory, urlEntityApi)
        {
        }

        public async Task<PeriodoCarreraDTO> GetCurrentPeriodoAsync()
        {
            PeriodoCarreraDTO periodo = null;
            HttpResponseMessage response = await _httpClient.GetAsync($"{_urlEntityApi}/Current");
            if (response.IsSuccessStatusCode)
            {
                periodo = await response.Content.ReadAsAsync<PeriodoCarreraDTO>();
            }
            return periodo;
        }
    }
}