using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Schedule.Entities;

namespace Schedule.DAO
{
    public class ProfesorMateriaDAO
    {
        private DBConnection _connection = new DBConnection();

        public ProfesorMateriaDAO()
        {
            _connection.CreateMySqlConnection();
        }

        /// <summary>
        /// Crea una nueva relacion entre profesor y materia
        /// </summary>
        /// <param name="cedula">Cedula del profesor</param>
        /// <param name="codigo">Codigo de la materia</param>
        /// <returns>True en caso de exito</returns>
        public bool Create(int cedula, int codigo)
        {
            bool result = false;
            try
            {
                _connection.OpenConnection();
                _connection.CreateCommand("sp_CreateProfesorxMateria", CommandType.StoredProcedure);
                _connection.AssignParameter(true, "@cedula", cedula);
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
        /// Borra una relacion entre profesor y materia
        /// </summary>
        /// <param name="id">ID de la tabla</param>
        /// <returns>True en caso de exito</returns>
        public bool Delete(int id)
        {
            bool result = false;
            try
            {
                _connection.OpenConnection();
                _connection.CreateCommand("sp_DeleteProfesorxMateria", CommandType.StoredProcedure);
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
        /// Obtiene una lista de Profesores x Materias
        /// </summary>
        /// <returns>Lista de Profesores x Materias</returns>
        public List<ProfesorMateria> GetAll()
        {
            List<ProfesorMateria> lista = new List<ProfesorMateria>();
            try
            {
                _connection.OpenConnection();
                _connection.CreateCommand("sp_GetProfesoresxMaterias", CommandType.StoredProcedure);
                var result = _connection.ExecuteConsulta();
                while(result.Read())
                {
                    ProfesorMateria pm = new ProfesorMateria
                    {
                        Materia = new Materias 
                        {
                            Codigo = Convert.ToInt32("codigo"),
                            Nombre = (string)result["asignatura"],
                            Semestre = new SemestreDAO().Get(Convert.ToInt32(result["id_semestre"])),
                            Carrera = new CarreraDAO().Get(Convert.ToInt32(result["id_carrera"])),
                            TipoMateria = new TipoAulaMateriaDAO().Get(Convert.ToInt32(result["id_tipo_materia"]))
                        },
                        Profesor = new Profesor
                        {
                            Cedula = Convert.ToInt32("cedula"),
                            Nombre = (string)result["nombre"],
                            Apellido = (string)result["apellido"]
                        }
                    };
                    lista.Add(pm);
                }
            }
            catch (Exception)
            {
                return lista;
            }
            finally
            {
                if (_connection != null) _connection.CloseConnection();
            }
            return lista;
        }

        /// <summary>
        /// Actualiza una relacion entre profesor y materia
        /// </summary>
        /// <param name="cedula">Cedula del profesor</param>
        /// <param name="codigo">Codigo de la materia</param>
        /// <param name="cedulaNueva">Nueva cedula del profesor</param>
        /// <param name="codigoNuevo">Nuevo codigo de la materia</param>
        /// <returns>True en caso de exito</returns>
        public bool Update(int cedula, int codigo, int cedulaNueva, int codigoNuevo)
        {
            bool result = false;
            try
            {
                _connection.OpenConnection();
                _connection.CreateCommand("sp_UpdateProfesorxMateria", CommandType.StoredProcedure);
                _connection.AssignParameter(true, "@cedula", cedula);
                _connection.AssignParameter(true, "@codigo", codigo);
                _connection.AssignParameter(true, "@cedulaNueva", cedulaNueva);
                _connection.AssignParameter(true, "@codigoNuevo", codigoNuevo);
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
