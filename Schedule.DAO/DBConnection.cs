using MySql.Data.MySqlClient;
using System.Data;

namespace Schedule.DAO
{
    public class DBConnection
    {
        const string myConnString = "server=localhost;userid=wolfteam20;pwd=220770;port=3306;database=horarios;sslmode=none;";
        private MySqlConnection _connection = null;
        private MySqlCommand _command;

        /// <summary>
        /// Crea una conexion con la base de datos MySQL
        /// </summary>
        /// <returns></returns>
        public MySqlConnection CreateMySqlConnection()
        {
            if (_connection == null)
            {
                _connection = new MySqlConnection(myConnString);
            }
            return _connection;
        }

        /// <summary>
        /// Abre la conexion con la BD
        /// </summary>
        public void OpenConnection()
        {
            _connection.Open();
        }

        /// <summary>
        /// Cierra la conexion con la BD
        /// </summary>
        public void CloseConnection()
        {
            _connection.Close();
        }
        /// <summary>
        /// Crea un comando a ejecutar
        /// </summary>
        /// <param name="sql">Sentencia sql o nombre del store procedure</param>
        /// <param name="commandType">Tipo de comando a ejecutar</param>
        public void CreateCommand(string sql, CommandType commandType)
        {
            _command = new MySqlCommand
            {
                Connection = _connection,
                CommandText = sql,
                CommandType = commandType
            };
        }

        /// <summary>
        /// Asigna un parametro a un comando
        /// </summary>
        /// <param name="isStoredProcedure">Es store procedure?</param>
        /// <param name="parametersName">Nombre del parametro (@parametro)</param>
        /// <param name="parametersValue">Valor del parametro</param>
        /// <param name="parametersDirection">Direccion del parametro (input/output)</param>
        /// <param name="parametersType">Tipo del parametro</param>
        public void AssignParameter(bool isStoredProcedure, string parametersName, object parametersValue,
            ParameterDirection parametersDirection = ParameterDirection.Input, DbType parametersType = DbType.Object)
        {
            if (isStoredProcedure)
            {
                MySqlParameter parametro = new MySqlParameter
                {
                    Direction = parametersDirection,
                    ParameterName = parametersName,
                    Value = parametersValue
                };

                if (parametersType != DbType.Object) parametro.DbType = parametersType;

                _command.Parameters.Add(parametro);
            }
            else
            {
                MySqlParameter parametro = new MySqlParameter
                {
                    ParameterName = parametersName,
                    Value = parametersValue
                };
                _command.Parameters.Add(parametro);
            }
        }

        /// <summary>
        /// Ejecuta el commando que no devuelve filas
        /// </summary>
        /// <returns>Devuelve el numero de filas afectadas por el comando</returns>
        public int ExecuteCommand()
        {
            return _command.ExecuteNonQuery();
        }

        /// <summary>
        /// Ejecuta el comando el cual devuelve una lista de resultados
        /// </summary>
        /// <returns>MySqlDataReader</returns>
        public MySqlDataReader ExecuteConsulta()
        {
            return _command.ExecuteReader();
        }
    }
}
