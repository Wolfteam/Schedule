using Schedule.Entities;
using System;
using System.Collections.Generic;
using System.Data;

namespace Schedule.DAO
{
    public class CarreraDAO : ICommon<Carrera>
    {
        private DBConnection _connection = new DBConnection();

        public CarreraDAO()
        {
            _connection.CreateMySqlConnection();
        }

        public bool Create(Carrera objeto)
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
        /// Obtiene una carrera en particular
        /// </summary>
        /// <param name="id">ID de la carrera a buscar</param>
        /// <returns>Objeto Carrera</returns>
        public Carrera Get(int id)
        {
            Carrera carrera = null;
            try
            {
                _connection.CreateCommand("sp_GetCarreras", CommandType.StoredProcedure);
                _connection.AssignParameter(true, "@id", id);
                _connection.OpenConnection();

                var result = _connection.ExecuteConsulta();

                while (result.Read())
                {
                    carrera = new Carrera
                    {
                        ID = Convert.ToInt32(result["id_carrera"]),
                        NombreCarrera = (string)result["nombre_carrera"]
                    };
                }
            }
            catch (Exception)
            {
                return carrera;
            }
            finally
            {
                _connection.CloseConnection();
            }
            return carrera;
        }

        public List<Carrera> GetAll()
        {
            throw new NotImplementedException();
        }

        public bool Update(Carrera objeto)
        {
            throw new NotImplementedException();
        }

        public bool Update(int id, Carrera objeto)
        {
            throw new NotImplementedException();
        }
    }
}
