using Schedule.Entities;

namespace Schedule.Web.Models.Repository
{
    public class TipoAulaMateriaRepository: Repository<TipoAulaMateriaDTO, TipoAulaMateriaDTO>, ITipoAulaMateriaRepository
    {
        public TipoAulaMateriaRepository(IHttpClientsFactory httpClientsFactory, string urlEntityApi)
            : base(httpClientsFactory, urlEntityApi)
        {
        }
    }
}