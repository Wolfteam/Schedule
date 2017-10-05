using Schedule.Entities;
using System;
using System.Collections.Generic;
using System.Data;

namespace Schedule.DAO
{
    public class TipoAulaMateriaDAO : ICommon<TipoAulaMateria>
    {
        private DBConnection _connection = new DBConnection();

        public TipoAulaMateriaDAO()
        {
            _connection.CreateMySqlConnection();
        }

        public bool Create(TipoAulaMateria objeto)
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
        /// Obtiene un TipoAulaMateria en particular
        /// </summary>
        /// <param name="id">Id del TipoAulaMateria a buscar</param>
        /// <returns>Objeto TipoAulaMateria</returns>
        public TipoAulaMateria Get(int id)
        {
            TipoAulaMateria tipo = null;
            try
            {
                _connection.OpenConnection();
                _connection.CreateCommand("sp_GetTipoAulaMateria", CommandType.StoredProcedure);
                _connection.AssignParameter(true, "@id", id);

                var result = _connection.ExecuteConsulta();

                while (result.Read())
                {
                    tipo = new TipoAulaMateria
                    {
                        ID = Convert.ToInt32(result["id_tipo"]),
                        Nombre = (string)result["nombre_tipo"]
                    };
                }
            }
            catch (Exception)
            {
                return tipo;
            }
            finally
            {
                if (_connection != null) _connection.CloseConnection();
            }
            return tipo;
        }

        public List<TipoAulaMateria> GetAll()
        {
            throw new NotImplementedException();
        }

        public bool Update(TipoAulaMateria objeto)
        {
            throw new NotImplementedException();
        }

        public bool Update(int id, TipoAulaMateria objeto)
        {
            throw new NotImplementedException();
        }
    }
}
