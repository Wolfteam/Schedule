using Schedule.Entities;
using System.Net.Http;

namespace Schedule.Web.Models.Repository
{
    public class AulasRepository : Repository<AulasDTO, AulasDetailsDTO>, IAulasRepository
    {
        public AulasRepository(HttpClient httpClient, string urlEntityApi) 
            : base(httpClient, urlEntityApi)
        {
        }
    }
}