using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Diagnostics;
using Schedule.Entities;
using System.Collections.Generic;

namespace Schedule.DAO
{
    public class DiasDAO : ICommon<DiasHabiles>
    {
        private DBConnection _connection = new DBConnection();

        public DiasDAO()
        {
            _connection.CreateMySqlConnection();
        }

        public bool Create(DiasHabiles objeto)
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
        /// Obtiene la informacion de los dias para uno en particular
        /// </summary>
        /// <param name="id">Id del dia a buscar</param>
        /// <returns>Objeto de tipo DiasHabiles</returns>
        public DiasHabiles Get(int id)
        {
            DiasHabiles dia = null;
            try
            {
                _connection.OpenConnection();
                _connection.CreateCommand("sp_GetDiasHabiles", CommandType.StoredProcedure);
                _connection.AssignParameter(true, "@id", id);

                var result = _connection.ExecuteConsulta();

                while (result.Read())
                {
                    dia = new DiasHabiles
                    {
                        ID = Convert.ToInt32(result["id_dia"]),          
                        NombreDia = (string)result["nombre_dia"]
                    };
                }
            }
            catch (Exception)
            {
                return dia;
            }
            finally
            {
                if (_connection != null) _connection.CloseConnection();
            }
            return dia;
        }

        public List<DiasHabiles> GetAll()
        {
            throw new NotImplementedException();
        }

        public bool Update(DiasHabiles objeto)
        {
            throw new NotImplementedException();
        }

        public bool Update(int id, DiasHabiles objeto)
        {
            throw new NotImplementedException();
        }
    }
}
