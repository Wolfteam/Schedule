using Schedule.Entities;
using System.Net.Http;

namespace Schedule.Web.Models.Repository
{
    public class CarrerasRepository : Repository<CarreraDTO, CarreraDTO>, ICarrerasRepository
    {
        public CarrerasRepository(HttpClient httpClient, string urlEntityApi) 
            : base(httpClient, urlEntityApi)
        {
        }
    }
}