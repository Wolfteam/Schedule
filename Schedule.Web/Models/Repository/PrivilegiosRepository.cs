using Schedule.Entities;

namespace Schedule.Web.Models.Repository
{
    public class PrivilegiosRepository : Repository<PrivilegiosDTO, PrivilegiosDTO>, IPrivilegiosRepository
    {
        public PrivilegiosRepository(IHttpClientsFactory httpClientsFactory, string urlEntityApi) 
            : base(httpClientsFactory, urlEntityApi)
        {
        }
    }
}