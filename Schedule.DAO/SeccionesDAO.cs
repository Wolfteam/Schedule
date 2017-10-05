using Schedule.Entities;
using System;
using System.Collections.Generic;
using System.Data;

namespace Schedule.DAO
{
    public class SeccionesDAO : ICommon<Secciones>
    {
        private DBConnection _connection = new DBConnection();

        public SeccionesDAO()
        {
            _connection.CreateMySqlConnection();
        }

        /// <summary>
        /// Crea una nueva seccion
        /// </summary>
        /// <param name="seccion">Objeto de tipo seccion</param>
        /// <returns>True en caso de exito</returns>
        public bool Create(Secciones seccion)
        {
            bool result = false;
            try
            {
                _connection.OpenConnection();
                _connection.CreateCommand("sp_CreateSecciones", CommandType.StoredProcedure);
                _connection.AssignParameter(true, "@codigo", seccion.CodigoMateria);
                _connection.AssignParameter(true, "@numeroSecciones", seccion.NumeroSecciones);
                _connection.AssignParameter(true, "@cantidadAlumnos", seccion.CantidadAlumnos);
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
        /// Borra una seccion en especifico
        /// </summary>
        /// <param name="codigo">Codigo de la materia a la cual se le eliminaran las secciones</param>
        /// <returns>True en caso de exito</returns>
        public bool Delete(int codigo)
        {
            bool result = false;
            try
            {
                _connection.OpenConnection();
                _connection.CreateCommand("sp_DeleteSecciones", CommandType.StoredProcedure);
                _connection.AssignParameter(true, "@codigo", codigo);
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
        /// Borra todas las secciones
        /// </summary>
        /// <returns>True en caso de exito</returns>
        public bool Delete()
        {
            bool result = false;
            try
            {
                _connection.OpenConnection();
                _connection.CreateCommand("sp_DeleteSecciones", CommandType.StoredProcedure);
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
        /// Obtiene una seccion en particular
        /// </summary>
        /// <param name="codigo">Id de la materia a buscar</param>
        /// <returns>Objeto Secciones</returns>
        public Secciones Get(int codigo)
        {
            Secciones seccion = null;
            try
            {
                _connection.OpenConnection();
                _connection.CreateCommand("sp_GetSecciones", CommandType.StoredProcedure);
                _connection.AssignParameter(true, "@codigo", codigo);

                var result = _connection.ExecuteConsulta();

                while (result.Read())
                {
                    seccion = new Secciones
                    {
                         CodigoMateria = codigo,
                         NumeroSecciones = Convert.ToInt32(result["numero_secciones"]),
                         CantidadAlumnos = Convert.ToInt32(result["cantidad_alumnos"])
                    };
                }
            }
            catch (Exception)
            {
                return seccion;
            }
            finally
            {
                if (_connection != null) _connection.CloseConnection();
            }
            return seccion;
        }


        /// <summary>
        /// Obtiene una lista de todas las secciones
        /// </summary>
        /// <returns>Lista de secciones</returns>
        public List<Secciones> GetAll()
        {
            List<Secciones> listaSecciones = new List<Secciones>();
            try
            {
                _connection.OpenConnection();
                _connection.CreateCommand("sp_GetSecciones", CommandType.StoredProcedure);

                var result = _connection.ExecuteConsulta();

                while (result.Read())
                {
                    Secciones seccion = new Secciones
                    {
                        CodigoMateria = Convert.ToInt32(result["codigo"]),
                        NumeroSecciones = Convert.ToInt32(result["numero_secciones"]),
                        CantidadAlumnos = Convert.ToInt32(result["cantidad_alumnos"])
                    };
                    listaSecciones.Add(seccion);
                }
            }
            catch (Exception)
            {
                return listaSecciones;
            }
            finally
            {
                if (_connection != null) _connection.CloseConnection();
            }
            return listaSecciones;
        }

        /// <summary>
        /// Actualiza una seccion en especifico
        /// </summary>
        /// <param name="seccion">Objeto seccion a actualizar</param>
        /// <returns>True en caso de exito</returns>
        public bool Update(int codigo, Secciones seccion)
        {
            bool result = false;
            try
            {
                _connection.OpenConnection();
                _connection.CreateCommand("sp_UpdateSecciones", CommandType.StoredProcedure);
                _connection.AssignParameter(true, "@codigo", codigo);
                _connection.AssignParameter(true, "@codigoNuevo", seccion.CodigoMateria);
                _connection.AssignParameter(true, "@cantidadAlumnos", seccion.CantidadAlumnos);
                _connection.AssignParameter(true, "@numeroSecciones", seccion.NumeroSecciones);

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
    }
}
