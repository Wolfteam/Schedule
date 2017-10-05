using Schedule.Entities;
using System;
using System.Collections.Generic;
using System.Data;

namespace Schedule.DAO
{
    public class ProfesorDAO : ICommon<Profesor>
    {
        private DBConnection _connection = new DBConnection();

        public ProfesorDAO()
        {
            _connection.CreateMySqlConnection();
        }

        /// <summary>
        /// Crea un nuevo profesor
        /// </summary>
        /// <param name="profesor">Objeto de tipo profesor</param>
        /// <returns>True en caso de exito</returns>
        public bool Create(Profesor profesor)
        {
            bool result = false;
            try
            {
                _connection.OpenConnection();
                _connection.CreateCommand("sp_CreateProfesor", CommandType.StoredProcedure);
                _connection.AssignParameter(true, "@cedula", profesor.Cedula);
                _connection.AssignParameter(true, "@nombre", profesor.Nombre);
                _connection.AssignParameter(true, "@apellido", profesor.Apellido);
                _connection.AssignParameter(true, "@idPrioridad", profesor.Prioridad.ID);

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
        /// Obtiene un profesor en particular
        /// </summary>
        /// <param name="cedula">Id del profesor a buscar</param>
        /// <returns>Objeto Profesor</returns>
        public Profesor Get(int cedula)
        {
            Profesor profesor = null;
            try
            {
                _connection.OpenConnection();
                _connection.CreateCommand("sp_GetProfesores", CommandType.StoredProcedure);
                _connection.AssignParameter(true, "@cedula", cedula);

                var result = _connection.ExecuteConsulta();

                while (result.Read())
                {
                    profesor = new Profesor
                    {
                        Cedula = cedula,
                        Nombre = (string)result["nombre"],
                        Apellido = (string)result["apellido"],
                        Prioridad = GetPrioridad(cedula)
                    };
                }
            }
            catch (Exception)
            {
                return profesor;
            }
            finally
            {
                if (_connection != null) _connection.CloseConnection();
            }
            return profesor;
        }

        /// <summary>
        /// Obtiene una lista de profesores
        /// </summary>
        /// <returns>Lista de profesores</returns>
        public List<Profesor> GetAll()
        {
            List<Profesor> listaProfesores = new List<Profesor>();
            try
            {
                _connection.OpenConnection();
                _connection.CreateCommand("sp_GetProfesores", CommandType.StoredProcedure);
                _connection.AssignParameter(true, "@cedula", null);

                var result = _connection.ExecuteConsulta();

                while (result.Read())
                {
                    Profesor aula = new Profesor
                    {
                        Cedula = Convert.ToInt32(result["cedula"]),
                        Nombre = (string)result["nombre"],
                        Apellido = (string)result["apellido"],
                        Prioridad = GetPrioridad(Convert.ToInt32(result["cedula"]))
                    };
                    listaProfesores.Add(aula);
                }
            }
            catch (Exception)
            {
                return listaProfesores;
            }
            finally
            {
                if (_connection != null) _connection.CloseConnection();
            }
            return listaProfesores;
        }

        /// <summary>
        /// Borra una profesor en especifico
        /// </summary>
        /// <param name="cedula">Id del profesor a eliminar</param>
        /// <returns>True en caso de exito</returns>
        public bool Delete(int cedula)
        {
            bool result = false;
            try
            {
                _connection.OpenConnection();
                _connection.CreateCommand("sp_DeleteProfesores", CommandType.StoredProcedure);
                _connection.AssignParameter(true, "@cedula", cedula);
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
        /// Actualiza un profesor en especifico
        /// </summary>
        /// <param name="profesor">Objeto profesor a actualizar</param>
        /// <returns>True en caso de exito</returns>
        public bool Update(int cedula, Profesor profesor)
        {
            bool result = false;
            try
            {
                _connection.OpenConnection();
                _connection.CreateCommand("sp_UpdateProfesores", CommandType.StoredProcedure);
                _connection.AssignParameter(true, "@cedula", cedula);
                _connection.AssignParameter(true, "@apellido", profesor.Apellido);
                _connection.AssignParameter(true, "@cedulaNueva", profesor.Cedula);
                _connection.AssignParameter(true, "@idPrioridad", profesor.Prioridad.ID);
                _connection.AssignParameter(true, "@nombre", profesor.Nombre);
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

        /// <summary>
        /// Obtiene la prioridad de un profesor en particular
        /// </summary>
        /// <param name="cedula">Cedula del profesor</param>
        /// <returns>Objeto de tipo PrioridadProfesor</returns>
        public PrioridadProfesor GetPrioridad(int cedula)
        {
            PrioridadProfesor prioridad = null;
            try
            {
                _connection.OpenConnection();
                _connection.CreateCommand("sp_GetPrioridad", CommandType.StoredProcedure);
                _connection.AssignParameter(true, "@cedula", cedula);

                var result = _connection.ExecuteConsulta();

                while (result.Read())
                {
                    prioridad = new PrioridadProfesor
                    {
                        ID = Convert.ToInt32(result["id_prioridad"]),
                        CodigoPrioridad = (string)result["codigo_prioridad"],
                        HorasACumplir = Convert.ToInt32(result["horas_a_cumplir"])
                    };
                }
            }
            catch (Exception)
            {
                return prioridad;
            }
            finally
            {
                if (_connection != null) _connection.CloseConnection();
            }
            return prioridad;
        }
    }
}
