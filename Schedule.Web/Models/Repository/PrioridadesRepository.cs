using Schedule.Entities;

namespace Schedule.Web.Models.Repository
{
    public class PrioridadesRepository : Repository<PrioridadProfesorDTO, PrioridadProfesorDTO>, IPrioridadesRepository
    {
        public PrioridadesRepository(IHttpClientsFactory httpClientsFactory, string urlEntityApi) 
            : base(httpClientsFactory, urlEntityApi)
        {
        }
    }
}