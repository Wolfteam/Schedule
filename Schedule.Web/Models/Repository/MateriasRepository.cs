using Schedule.Entities;

namespace Schedule.Web.Models.Repository
{
    public class MateriasRepository : Repository<MateriasDTO, MateriasDetailsDTO>, IMateriasRepository
    {
        public MateriasRepository(IHttpClientsFactory httpClientsFactory, string urlEntityApi) 
            : base(httpClientsFactory, urlEntityApi)
        {
        }
    }
}