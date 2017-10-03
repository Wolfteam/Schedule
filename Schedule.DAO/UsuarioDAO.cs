using System;
using System.Data;

namespace Schedule.DAO
{
    public class UsuarioDAO
    {
        private DBConnection _connection = new DBConnection();

        public UsuarioDAO()
        {
            _connection.CreateMySqlConnection();
        }

        /// <summary>
        /// Autentica un usuario contra la base de datos
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <returns>True en caso de existir</returns>
        public bool AuthenticateUser(string username, string password)
        {
            bool result = false;
            try
            {
                _connection.OpenConnection();
                _connection.CreateCommand("sp_AuthenticateUser", CommandType.StoredProcedure);
                _connection.AssignParameter(true, "@username", username);
                _connection.AssignParameter(true, "@password", password);
                result = _connection.ExecuteConsulta().HasRows;
            }
            catch (Exception)
            {
                return result;
            }
            finally
            {
                if (_connection != null) _connection.CloseConnection();
            }
            return result;
        }
    }
}
