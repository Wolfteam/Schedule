using Schedule.Entities;

namespace Schedule.Web.Models.Repository
{
    public class ProfesorMateriaRepository 
        : Repository<ProfesorMateriaDTO, ProfesorMateriaDetailsDTO>, IProfesorMateriaRepository
    {
        public ProfesorMateriaRepository(IHttpClientsFactory httpClientsFactory, string urlEntityApi) 
            : base(httpClientsFactory, urlEntityApi)
        {
        }
    }
}