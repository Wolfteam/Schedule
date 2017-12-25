using Schedule.Entities;

namespace Schedule.Web.Models.Repository
{
    public class ProfesorRepository : Repository<ProfesorDTO, ProfesorDetailsDTO>, IProfesorRepository
    {
        public ProfesorRepository(IHttpClientsFactory httpClientsFactory, string urlEntityApi)
            : base(httpClientsFactory, urlEntityApi)
        {
        }
    }
}