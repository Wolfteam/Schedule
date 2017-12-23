using Schedule.Entities;
using System.Net.Http;

namespace Schedule.Web.Models.Repository
{
    public class MateriasRepository : Repository<MateriasDTO, MateriasDetailsDTO>, IMateriasRepository
    {
        public MateriasRepository(HttpClient httpClient, string urlEntityApi) 
            : base(httpClient, urlEntityApi)
        {
        }
    }
}