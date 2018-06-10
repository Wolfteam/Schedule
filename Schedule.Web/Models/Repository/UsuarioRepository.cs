using System.Net.Http;
using System.Threading.Tasks;
using Schedule.Entities;

namespace Schedule.Web.Models.Repository
{
    public class UsuarioRepository : Repository<UsuarioDTO, UsuarioDetailsDTO>, IUsuarioRepository
    {
        public UsuarioRepository(IHttpClientsFactory httpClientsFactory, string urlEntityApi)
            : base(httpClientsFactory, urlEntityApi)
        {
        }

        public async Task<ResultDTO> ChangePassword(uint cedula, ChangePasswordDTO request)
        {
            var response = await _httpClient
                .PutAsJsonAsync($"{_urlEntityApi}/{cedula}/ChangePassword", request);
            var result = await response.Content.ReadAsAsync<ResultDTO>();
            return result;
        }
    }
}