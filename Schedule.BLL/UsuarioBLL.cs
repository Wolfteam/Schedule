using Schedule.DAO;

namespace Schedule.BLL
{
    public class UsuarioBLL
    {
        public bool AuthenticateUser(string username, string password)
        {
            return new UsuarioDAO().AuthenticateUser(username, password);
        }
    }
}
