using Schedule.Entities;

namespace Schedule.Web.Models.Repository
{
    public class AulasRepository : Repository<AulasDTO, AulasDetailsDTO>, IAulasRepository
    {
        public AulasRepository(IHttpClientsFactory httpClientsFactory, string urlEntityApi) 
            : base(httpClientsFactory, urlEntityApi)
        {
        }
    }
}