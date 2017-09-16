using Microsoft.AspNetCore.Mvc.Filters;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Schedule.DAO
{
    public class UsuarioDAO
    {
        public UsuarioDAO()
        {
        }

        public bool AuthenticateUser(string username, string password)
        {
            DBConnection connection = new DBConnection();
            connection.CreateMySqlConnection();
            connection.CreateCommand("sp_AuthenticateUser", CommandType.StoredProcedure);
            connection.AssignParameter(true, "@username", username);
            connection.AssignParameter(true, "@password", password);
            try
            {
                connection.OpenConnection();
            }
            catch (Exception)
            {
                return false;
            }
            bool result = connection.ExecuteConsulta().HasRows;
            connection.CloseConnection();
            return result;
        }

    }
}
