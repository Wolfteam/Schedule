using Schedule.Entities;

namespace Schedule.Web.Models.Repository
{
    public class CarrerasRepository : Repository<CarreraDTO, CarreraDTO>, ICarrerasRepository
    {
        public CarrerasRepository(IHttpClientsFactory httpClientsFactory, string urlEntityApi) 
            : base(httpClientsFactory, urlEntityApi)
        {
        }
    }
}