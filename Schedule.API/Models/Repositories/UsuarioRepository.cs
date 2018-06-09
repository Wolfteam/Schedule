using Microsoft.EntityFrameworkCore;
using Schedule.Entities;
using System.Linq;
using AutoMapper.QueryableExtensions;

namespace Schedule.API.Models.Repositories
{
    public class UsuarioRepository : Repository<Admin>, IUsuarioRepository
    {
        public HorariosContext HorariosContext
        {
            get { return _context as HorariosContext; }
        }

        public UsuarioRepository(DbContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Obtiene un usuario en particular en base a su cedula
        /// </summary>
        /// <param name="cedula">Cedula del usuario</param>
        /// <returns>UsuarioDetailsDTO</returns>
        public UsuarioDetailsDTO Get(uint cedula)
        {
            return HorariosContext.Admin
                .Where(u => u.Cedula == cedula)
                .ProjectTo<UsuarioDetailsDTO>()
                .FirstOrDefault();
        }

        /// <summary>
        /// Autentica un usuario contra la base de datos
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <returns>True en caso de existir</returns>
        public bool UserExists(string username, string password)
        {
            var usuarioExist = HorariosContext.Admin
                .AsNoTracking()
                .Where(x => x.Username == username && x.Password == password)
                .Count() == 1;
            if (usuarioExist)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Verifica si un usuario en particular es administrador o no
        /// </summary>
        /// <param name="username">Usuario a verificar</param>
        /// <returns>True en caso de ser administrador</returns>
        public bool IsUserAdmin(string username)
        {
            var privilegio = (Entities.Privilegios)HorariosContext.Admin.FirstOrDefault(u => u.Username == username).IdPrivilegio;
            return privilegio.Equals(Entities.Privilegios.Administrador);
        }

        /// <summary>
        /// Verifica si la <paramref name="password"/> corresponde a la <paramref name="cedula"/> dada
        /// </summary>
        /// <param name="cedula">Cedula del usuario</param>
        /// <param name="password">Password asociada a la cedula</param>
        /// <returns>True en caso de que la cedula tenga el password indicado</returns>
        public bool IsCurrentPasswordValid(uint cedula, string password)
        {
            bool isCurrentPasswordValid = HorariosContext.Admin
                .AsNoTracking()
                .Where(x => x.Cedula == cedula && x.Password == password)
                .Count() == 1;
            return isCurrentPasswordValid;
        }

        /// <summary>
        /// Actualiza el password de un usuario en particular
        /// </summary>
        /// <param name="cedula">Cedula del usuario</param>
        /// <param name="newPassowrd">Password a colocar</param>
        public void ChangePassword(uint cedula, string newPassowrd)
        {
            var usuario = base.Get(cedula);
            usuario.Password = newPassowrd;
            Update(usuario);
        }
    }
}
