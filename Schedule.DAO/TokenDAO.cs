using Schedule.Entities;
using System;
using System.Collections.Generic;
using System.Data;

namespace Schedule.DAO
{
    public class TokenDAO
    {
        private DBConnection _connection = new DBConnection();

        public TokenDAO()
        {
            _connection.CreateMySqlConnection();
        }

        /// <summary>
        /// Guarda el token generado en la base de datos
        /// </summary>
        /// <param name="token">Objeto de tipo Token</param>
        public void SaveToken(Token token)
        {
            try
            {
                _connection.OpenConnection();
                _connection.CreateCommand("sp_SaveToken", CommandType.StoredProcedure);
                _connection.AssignParameter(true, "@token", token.AuthenticationToken);
                _connection.AssignParameter(true, "@createDate", token.CreateDate);
                _connection.AssignParameter(true, "@expiricyDate", token.ExpiricyDate);
                _connection.AssignParameter(true, "@username", token.Username);
                _connection.ExecuteCommand();
            }
            catch (Exception)
            {
                //TODO esto deberia de devolver true o false en caso de exito
                return;
            }
            finally
            {
                if (_connection != null) _connection.CloseConnection();
            }
        }

        /// <summary>
        /// Valida si el token existe en la base de datos
        /// </summary>
        /// <param name="tokenID">Token de autenticacion</param>
        /// <returns>True en caso de existir</returns>
        public bool ValidateToken(string tokenID)
        {
            bool result = false;
            try
            {
                _connection.OpenConnection();
                _connection.CreateCommand("sp_ValidateToken", CommandType.StoredProcedure);
                _connection.AssignParameter(true, "@token", tokenID);
                _connection.AssignParameter(true, "@expiricyDate", DateTime.Now);
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

        /// <summary>
        /// Obtiene un objeto Token acorde al tokenID
        /// </summary>
        /// <param name="tokenID">TokenId</param>
        /// <returns>Objeto de la clase Token</returns>
        public Token GetToken(string tokenID)
        {
            Token token = null;
            try
            {
                token = new Token();
                _connection.OpenConnection();
                _connection.CreateCommand("sp_GetToken", CommandType.StoredProcedure);
                _connection.AssignParameter(true, "@token", tokenID);
                var result = _connection.ExecuteConsulta();
                while (result.Read())
                {
                    token.AuthenticationToken = result["token"].ToString();
                    token.CreateDate = Convert.ToDateTime(result["fecha_creacion"]);
                    token.ExpiricyDate = Convert.ToDateTime(result["fecha_expiracion"]);
                    token.Username = result["username"].ToString();
                }
            }
            catch (Exception)
            {
                return token;
            }
            finally
            {
                if (_connection != null) _connection.CloseConnection();
            }
            return token;
        }

        /// <summary>
        /// Actualiza un token en la base de datos
        /// </summary>
        /// <param name="token">Objeto de tipo Token</param>
        /// <returns>True en caso de exito</returns>
        public bool UpdateToken(Token token)
        {
            bool result = false;
            try
            {
                _connection.OpenConnection();
                _connection.CreateCommand("sp_UpdateToken", CommandType.StoredProcedure);
                _connection.AssignParameter(true, "@token", token.AuthenticationToken);
                _connection.AssignParameter(true, "@createDate", token.CreateDate);
                _connection.AssignParameter(true, "@expiricyDate", token.ExpiricyDate);
                _connection.AssignParameter(true, "@username", token.Username);
                result = _connection.ExecuteConsulta().RecordsAffected > 0;
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

        /// <summary>
        /// Elimina el token indicado de la base de datos
        /// </summary>
        /// <param name="tokenID">Token a eliminar</param>
        /// <returns>True en caso de exito</returns>
        public bool DeleteToken(string tokenID)
        {
            bool result = false;
            try
            {
                _connection.OpenConnection();
                _connection.CreateCommand("sp_DeleteToken", CommandType.StoredProcedure);
                _connection.AssignParameter(true, "@token", tokenID);
                result = _connection.ExecuteConsulta().RecordsAffected > 0;
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

        /// <summary>
        /// Obtiene una lista de privilegios asociada a un token
        /// </summary>
        /// <param name="tokenID">Token</param>
        /// <returns>Lista de tipo Privilegios</returns>
        public List<Privilegios> PrivilegiosByToken(string tokenID)
        {
            List<Privilegios> listaPrivilegios = null;
            try
            {
                listaPrivilegios = new List<Privilegios>();
                _connection.OpenConnection();
                _connection.CreateCommand("sp_PrivilegiosByToken", CommandType.StoredProcedure);
                _connection.AssignParameter(true, "@token", tokenID);
                var result = _connection.ExecuteConsulta();
                while (result.Read())
                {
                    //Aca no tiene mucho chiste ya que la lista siempre contendra un solo privilegio
                    listaPrivilegios.Add((Privilegios)Convert.ToInt32(result["Privilegio"]));
                }
            }
            catch (Exception)
            {
                return listaPrivilegios;
            }
            finally
            {
                if (_connection != null) _connection.CloseConnection();
            }
            return listaPrivilegios;
        }
    }
}
