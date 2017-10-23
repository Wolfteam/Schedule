using Schedule.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Schedule.API.Models.Repositories
{
    public class UsuarioRepository : IRepository<Admin, UsuarioDTO>
    {
        private readonly HorariosContext _db = new HorariosContext();

        public bool Create(Admin objeto)
        {
            throw new NotImplementedException();
        }

        public bool Delete()
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<UsuarioDTO> Get()
        {
            throw new NotImplementedException();
        }

        public UsuarioDTO Get(int cedula)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Autentica un usuario contra la base de datos
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <returns>True en caso de existir</returns>
        public bool Get(string username, string password)
        {
            if (_db.Admin.FirstOrDefault(x => x.Username == username && x.Password == password) != null)
            {
                return true;
            }
            return false;
        }

        public bool Update(int id, Admin objeto)
        {
            throw new NotImplementedException();
        }

        public bool Update(Admin objeto)
        {
            throw new NotImplementedException();
        }

    }
}
