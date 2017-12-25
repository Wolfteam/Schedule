using Schedule.Entities;

namespace Schedule.Web.Models.Repository
{
    public class SemestresRepository: Repository<SemestreDTO, SemestreDTO>, ISemestresRepository
    {
        public SemestresRepository(IHttpClientsFactory httpClientsFactory, string urlEntityApi)
            : base(httpClientsFactory, urlEntityApi)
        {
        }
    }
}