using System.Threading.Tasks;
using Schedule.Entities;

namespace Schedule.Web.Models.Repository
{
    public interface IUsuarioRepository : IRepository<UsuarioDTO, UsuarioDetailsDTO>
    {
        /// <summary>
        /// Cambia la password a un usuario en particular
        /// </summary>
        /// <param name="cedula">Cedula del usuario</param>
        /// <param name="request">Objeto que contiene la password vieja y nueva</param>
        /// <returns>ResultDTO con el resultado de la operacion</returns>
        Task<ResultDTO> ChangePassword(uint cedula, ChangePasswordDTO request);
    }
}
