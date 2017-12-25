using Schedule.Entities;

namespace Schedule.Web.Models.Repository
{
    public class DisponibilidadProfesorRepository
        : Repository<DisponibilidadProfesorDTO, DisponibilidadProfesorDetailsDTO>, IDisponibilidadProfesorRepository
    {
        public DisponibilidadProfesorRepository(IHttpClientsFactory httpClientsFactory, string urlEntityApi) 
            : base(httpClientsFactory, urlEntityApi)
        {
        }
    }
}