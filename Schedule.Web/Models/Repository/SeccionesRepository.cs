using Schedule.Entities;
using System.Net.Http;

namespace Schedule.Web.Models.Repository
{
    public class SeccionesRepository
        : Repository<SeccionesDTO, SeccionesDetailsDTO>, ISeccionesRepository
    {
        public SeccionesRepository(HttpClient httpClient, string urlEntityApi)
            : base(httpClient, urlEntityApi)
        {
        }
    }
}