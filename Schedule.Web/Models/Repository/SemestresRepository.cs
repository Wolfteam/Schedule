using Schedule.Entities;
using System.Net.Http;

namespace Schedule.Web.Models.Repository
{
    public class SemestresRepository
        : Repository<SemestreDTO, SemestreDTO>, ISemestresRepository
    {
        public SemestresRepository(HttpClient httpClient, string urlEntityApi)
            : base(httpClient, urlEntityApi)
        {
        }
    }
}