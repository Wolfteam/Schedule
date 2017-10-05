using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Diagnostics;
using Schedule.Entities;
using System.Collections.Generic;

namespace Schedule.DAO
{
    public class HorasDAO : ICommon<Horas>
    {
        private DBConnection _connection = new DBConnection();

        public HorasDAO()
        {
            _connection.CreateMySqlConnection();

        }

        public bool Create(Horas objeto)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public bool Delete()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtiene la informacion de las horas para una en particular
        /// </summary>
        /// <param name="id">Id de la hora a buscar</param>
        /// <returns>Objeto de tipo Horas</returns>
        public Horas Get(int id)
        {
            Horas horas = null;
            try
            {
                _connection.OpenConnection();
                _connection.CreateCommand("sp_GetHoras", CommandType.StoredProcedure);
                _connection.AssignParameter(true, "@id", id);

                var result = _connection.ExecuteConsulta();

                while (result.Read())
                {
                    horas = new Horas
                    {
                        ID = Convert.ToInt32(result["id_hora"]),          
                        Hora = (string)result["nombre_hora"]
                    };
                }
            }
            catch (Exception)
            {
                return horas;
            }
            finally
            {
                if (_connection != null) _connection.CloseConnection();
            }
            return horas;
        }

        public List<Horas> GetAll()
        {
            throw new NotImplementedException();
        }

        public bool Update(Horas objeto)
        {
            throw new NotImplementedException();
        }

        public bool Update(int id, Horas objeto)
        {
            throw new NotImplementedException();
        }
    }
}