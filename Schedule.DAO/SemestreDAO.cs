using Schedule.Entities;
using System;
using System.Collections.Generic;
using System.Data;

namespace Schedule.DAO
{
    public class SemestreDAO : ICommon<Semestre>
    {
        private DBConnection _connection = new DBConnection();

        public SemestreDAO()
        {
            _connection.CreateMySqlConnection();
        }

        public bool Create(Semestre objeto)
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
        /// Obtiene la informacion de los semestres para uno en particular
        /// </summary>
        /// <param name="id">Id del semestre a buscar</param>
        /// <returns>Objeto de tipo Semestre</returns>
        public Semestre Get(int id)
        {
            Semestre semestre = null;
            try
            {
                _connection.CreateCommand("sp_GetSemestre", CommandType.StoredProcedure);
                _connection.AssignParameter(true, "@id", id);
                _connection.OpenConnection();

                var result = _connection.ExecuteConsulta();

                while (result.Read())
                {
                    semestre = new Semestre
                    {
                        ID = Convert.ToInt32(result["id_semestre"]),
                        NombreSemestre = (string)result["nombre_semestre"]
                    };
                }
            }
            catch (Exception)
            {
                return semestre;
            }
            finally
            {
                _connection.CloseConnection();
            }
            return semestre;
        }

        public List<Semestre> GetAll()
        {
            throw new NotImplementedException();
        }

        public bool Update(Semestre objeto)
        {
            throw new NotImplementedException();
        }

        public bool Update(int id, Semestre objeto)
        {
            throw new NotImplementedException();
        }
    }

}