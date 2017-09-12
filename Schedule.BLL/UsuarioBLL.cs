using Schedule.DAO;
using System;

namespace Schedule.BLL
{
    public class UsuarioBLL
    {
        public bool AuthenticateUser(string username, string password)
        {
            UsuarioDAO usuario = new UsuarioDAO();
            return usuario.AuthenticateUser(username, password);           
        }
    }
}
