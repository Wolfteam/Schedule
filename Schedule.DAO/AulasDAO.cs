using Schedule.Entities;
using System;
using System.Collections.Generic;
using System.Data;

namespace Schedule.DAO
{
    public class AulasDAO : ICommon<Aulas>
    {
        private DBConnection _connection = new DBConnection();

        public AulasDAO()
        {
            _connection.CreateMySqlConnection();
        }

        /// <summary>
        /// Crea una nueva aula
        /// </summary>
        /// <param name="aula">Objeto de tipo Aula</param>
        /// <returns>True en caso de exito</returns>
        public bool Create(Aulas aula)
        {
            bool result = false;
            try
            {
                _connection.OpenConnection();
                _connection.CreateCommand("sp_CreateAulas", CommandType.StoredProcedure);
                _connection.AssignParameter(true, "@nombre", aula.Nombre);
                _connection.AssignParameter(true, "@capacidad", aula.Capacidad);
                _connection.AssignParameter(true, "@tipo", aula.IDTipoAula);
                result = _connection.ExecuteCommand() > 0 ? true : false;
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
        /// Obtiene un aula en particular
        /// </summary>
        /// <param name="id">Id del aula a buscar</param>
        /// <returns></returns>
        public Aulas Get(int id)
        {
            Aulas aula = null;
            try
            {
                _connection.OpenConnection();
                _connection.CreateCommand("sp_GetAulas", CommandType.StoredProcedure);
                _connection.AssignParameter(true, "@id", id);

                var result = _connection.ExecuteConsulta();

                while (result.Read())
                {
                    aula = new Aulas
                    {
                        Capacidad = Convert.ToInt32(result["capacidad"]),
                        IDTipoAula = Convert.ToInt32(result["id_tipo"]),
                        Nombre = (string)result["nombre_aula"]
                    };
                }
            }
            catch (Exception)
            {
                return aula;
            }
            finally
            {
                if (_connection != null) _connection.CloseConnection();
            }
            return aula;
        }

        /// <summary>
        /// Obtiene todas las aulas
        /// </summary>
        /// <returns>Lista de aulas</returns>
        public List<Aulas> GetAll()
        {
            List<Aulas> listaAulas = new List<Aulas>();
            try
            {
                _connection.OpenConnection();
                _connection.CreateCommand("sp_GetAulas", CommandType.StoredProcedure);
                _connection.AssignParameter(true, "@id", null);

                var result = _connection.ExecuteConsulta();

                while (result.Read())
                {
                    Aulas aula = new Aulas
                    {
                        Capacidad = Convert.ToInt32(result["capacidad"]),
                        IDTipoAula = Convert.ToInt32(result["id_tipo"]),
                        Nombre = (string)result["nombre_aula"]
                    };
                    listaAulas.Add(aula);
                }
            }
            catch (Exception)
            {
                return listaAulas;
            }
            finally
            {
                if (_connection != null) _connection.CloseConnection();
            }
            return listaAulas;
        }

        /// <summary>
        /// Borra un aula especifica
        /// </summary>
        /// <param name="id">Id del aula a eliminar</param>
        /// <returns>True en caso de exito</returns>
        public bool Delete(int id)
        {
            bool result = false;
            try
            {
                _connection.OpenConnection();
                _connection.CreateCommand("sp_DeleteAulas", CommandType.StoredProcedure);
                _connection.AssignParameter(true, "@id", id);
                result = _connection.ExecuteCommand() > 0 ? true : false;
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
        /// Actualiza una aula en especifico
        /// </summary>
        /// <param name="aula">Objeto aula a actualizar</param>
        /// <returns>True en caso de exito</returns>
        public bool Update(Aulas aula)
        {
            bool result = false;
            try
            {
                _connection.OpenConnection();
                _connection.CreateCommand("sp_UpdateAulas", CommandType.StoredProcedure);
                _connection.AssignParameter(true, "@capacidad", aula.Capacidad);
                _connection.AssignParameter(true, "@id", aula.ID);
                _connection.AssignParameter(true, "@idTipoAula", aula.IDTipoAula);
                _connection.AssignParameter(true, "@nombre", aula.Nombre);
                result = _connection.ExecuteCommand() > 0 ? true : false;
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

        public bool Delete()
        {
            throw new NotImplementedException();
        }
    }
}
