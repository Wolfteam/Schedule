﻿using Microsoft.EntityFrameworkCore;
using Schedule.Entities;
using System.Linq;

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
        /// Autentica un usuario contra la base de datos
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <returns>True en caso de existir</returns>
        public bool UserExists(string username, string password)
        {
            var usuario = HorariosContext.Admin.FirstOrDefault(x => x.Username == username && x.Password == password);
            if (usuario != null)
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
    }
}
