using Schedule.Entities;

namespace Schedule.Web.Models.Repository
{
    public class SeccionesRepository
        : Repository<SeccionesDTO, SeccionesDetailsDTO>, ISeccionesRepository
    {
        public SeccionesRepository(IHttpClientsFactory httpClientsFactory, string urlEntityApi)
            : base(httpClientsFactory, urlEntityApi)
        {
        }
    }
}