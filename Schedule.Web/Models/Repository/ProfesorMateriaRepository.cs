using Schedule.Entities;
using System.Net.Http;

namespace Schedule.Web.Models.Repository
{
    public class ProfesorMateriaRepository 
        : Repository<ProfesorMateriaDTO, ProfesorMateriaDetailsDTO>, IProfesorMateriaRepository
    {
        public ProfesorMateriaRepository(HttpClient httpClient, string urlEntityApi) 
            : base(httpClient, urlEntityApi)
        {
        }
    }
}