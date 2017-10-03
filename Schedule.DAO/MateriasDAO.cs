using Schedule.Entities;
using System;
using System.Collections.Generic;
using System.Data;

namespace Schedule.DAO
{
    public class MateriasDAO : ICommon<Materias>
    {
        private DBConnection _connection = new DBConnection();

        public MateriasDAO()
        {
            _connection.CreateMySqlConnection();
        }

        /// <summary>
        /// Crea una nueva materia
        /// </summary>
        /// <param name="materia">Objeto de tipo materia</param>
        /// <returns>True en caso de exito</returns>
        public bool Create(Materias materia)
        {
            bool result = false;
            try
            {
                _connection.OpenConnection();
                _connection.CreateCommand("sp_CreateMaterias", CommandType.StoredProcedure);
                _connection.AssignParameter(true, "@codigo", materia.Codigo);
                _connection.AssignParameter(true, "@nombre", materia.Nombre);
                _connection.AssignParameter(true, "@idCarrera", materia.IDCarrera);
                _connection.AssignParameter(true, "@idSemestre", materia.IDSemestre);
                _connection.AssignParameter(true, "@idTipoMateria", materia.IDTipoMateria);
                _connection.AssignParameter(true, "@horasSemanales", materia.HorasAcademicasSemanales);
                _connection.AssignParameter(true, "@horasAcademicasTotales", materia.HorasAcademicasTotales);
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
        /// Obtiene una materia en particular
        /// </summary>
        /// <param name="codigo">Id de la materia a buscar</param>
        /// <returns>Objeto Materias</returns>
        public Materias Get(int codigo)
        {
            Materias materia = null;
            try
            {
                _connection.OpenConnection();
                _connection.CreateCommand("sp_GetMaterias", CommandType.StoredProcedure);
                _connection.AssignParameter(true, "@codigo", codigo);

                var result = _connection.ExecuteConsulta();

                while (result.Read())
                {
                    materia = new Materias
                    {
                        Codigo = Convert.ToInt32(result["codigo"]),
                        Nombre = (string)result["asignatura"],
                        IDSemestre = Convert.ToInt32(result["id_semestre"]),
                        IDTipoMateria = Convert.ToInt32(result["id_tipo"]),
                        IDCarrera = Convert.ToInt32(result["id_carrera"]),
                        HorasAcademicasTotales = Convert.ToInt32(result["horas_academicas_totales"]),
                        HorasAcademicasSemanales = Convert.ToInt32(result["horas_academicas_semanales"])
                    };
                }
            }
            catch (Exception)
            {
                return materia;
            }
            finally
            {
                if (_connection != null) _connection.CloseConnection();
            }
            return materia;
        }

        /// <summary>
        /// Obtiene todas las materias
        /// </summary>
        /// <returns>Lista de materias</returns>
        public List<Materias> GetAll()
        {
            List<Materias> listaMaterias = new List<Materias>();
            try
            {
                _connection.OpenConnection();
                _connection.CreateCommand("sp_GetMaterias", CommandType.StoredProcedure);

                var result = _connection.ExecuteConsulta();

                while (result.Read())
                {
                    Materias aula = new Materias
                    {
                        Codigo = Convert.ToInt32(result["codigo"]),
                        Nombre = (string)result["asignatura"],
                        IDSemestre = Convert.ToInt32(result["id_semestre"]),
                        IDTipoMateria = Convert.ToInt32(result["id_tipo"]),
                        IDCarrera = Convert.ToInt32(result["id_carrera"]),
                        HorasAcademicasTotales = Convert.ToInt32(result["horas_academicas_totales"]),
                        HorasAcademicasSemanales = Convert.ToInt32(result["horas_academicas_semanales"])
                    };
                    listaMaterias.Add(aula);
                }
            }
            catch (Exception)
            {
                return listaMaterias;
            }
            finally
            {
                if (_connection != null) _connection.CloseConnection();
            }
            return listaMaterias;
        }

        /// <summary>
        /// Borra una materia en especifico
        /// </summary>
        /// <param name="id">Id de la materia a eliminar</param>
        /// <returns>True en caso de exito</returns>
        public bool Delete(int codigo)
        {
            bool result = false;
            try
            {
                _connection.OpenConnection();
                _connection.CreateCommand("sp_DeleteMaterias", CommandType.StoredProcedure);
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
        /// Actualiza una materia en especifico
        /// </summary>
        /// <param name="materia">Objeto materia a actualizar</param>
        /// <returns>True en caso de exito</returns>
        public bool Update(Materias materia)
        {
            bool result = false;
            try
            {
                _connection.OpenConnection();
                _connection.CreateCommand("sp_UpdateMaterias", CommandType.StoredProcedure);
                _connection.AssignParameter(true, "@codigo", materia.Codigo);
                _connection.AssignParameter(true, "@horasAcademicasSemanales", materia.HorasAcademicasSemanales);
                _connection.AssignParameter(true, "@horasAcademicasTotales", materia.HorasAcademicasTotales);
                _connection.AssignParameter(true, "@idCarrera", materia.IDCarrera);
                _connection.AssignParameter(true, "@idSemestre", materia.IDSemestre);
                _connection.AssignParameter(true, "@idTipoMateria", materia.IDTipoMateria);
                _connection.AssignParameter(true, "@nombre", materia.Nombre);
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
