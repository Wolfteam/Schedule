using Schedule.Entities;

namespace Schedule.Web.Models.Repository
{
    public class UsuarioRepository: Repository<UsuarioDTO, UsuarioDetailsDTO>, IUsuarioRepository
    {
        public UsuarioRepository(IHttpClientsFactory httpClientsFactory, string urlEntityApi)
            : base(httpClientsFactory, urlEntityApi)
        {
        }
    }
}