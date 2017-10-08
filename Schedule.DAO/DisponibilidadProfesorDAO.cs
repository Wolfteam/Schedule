using System;
using System.Data;
using Schedule.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace Schedule.DAO
{
    public class DisponibilidadProfesorDAO
    {
        private DBConnection _connection = new DBConnection();
        public DisponibilidadProfesorDAO()
        {
            _connection.CreateMySqlConnection();
        }
        //TODO: Ver si es mejor pasarle una lista y usar foreach o pasarlo asi como ta orita

        /// <summary>
        /// Crea una nueva disponibilidad a un profesor en particular
        /// </summary>
        /// <param name="disponibilidad">Objeto de tipo disponibilidad</param>
        /// <returns>True en caso de exito</returns>
        public bool Create(int cedula, int idDia, int idHoraInicio, int idHoraFin)
        {
            bool result = false;
            try
            {
                _connection.CreateCommand("sp_CreateDisponibilidadProfesor", CommandType.StoredProcedure);
                _connection.AssignParameter(true, "@cedula", cedula);
                _connection.AssignParameter(true, "@idDia", idDia);
                _connection.AssignParameter(true, "@idHoraInicio", idHoraInicio);
                _connection.AssignParameter(true, "@idHoraFin", idHoraFin);
                _connection.OpenConnection();

                result = _connection.ExecuteCommand() > 0 ? true : false;
            }
            catch (Exception)
            {
                return result;
            }
            finally
            {
                _connection.CloseConnection();
            }
            return result;
        }

        /// <summary>
        /// Borra todas las disponibilidades de un profesor
        /// </summary>
        /// <param name="cedula">Cedula del profesor</param>
        /// <returns>True en caso de exito</returns>
        public bool Delete(int cedula)
        {
            bool result = false;
            try
            {
                _connection.CreateCommand("sp_DeleteDisponibilidadProfesor", CommandType.StoredProcedure);
                _connection.AssignParameter(true, "@cedula", cedula);
                _connection.OpenConnection();

                result = _connection.ExecuteCommand() > 0 ? true : false;
            }
            catch (Exception)
            {
                return result;
            }
            finally
            {
                _connection.CloseConnection();
            }
            return result;
        }

        /// <summary>
        /// Borra todas las disponibilidades
        /// </summary>
        /// <returns>True en caso de exito</returns>
        public bool Delete()
        {
            bool result = false;
            try
            {
                _connection.CreateCommand("sp_DeleteDisponibilidadProfesor", CommandType.StoredProcedure);
                _connection.OpenConnection();

                result = _connection.ExecuteCommand() > 0 ? true : false;
            }
            catch (Exception)
            {
                return result;
            }
            finally
            {
                _connection.CloseConnection();
            }
            return result;

        }

        /// <summary>
        /// Obtiene una lista con todas las disponibilidad del profesor indicado
        /// </summary>
        /// <param name="cedula">Cedula del profesor</param>
        /// <returns>Lista de disponibilidades</returns>
        public DisponibilidadProfesor Get(int cedula)
        {
            DisponibilidadProfesor disponibilidad = new DisponibilidadProfesor();
            try
            {
                _connection.CreateCommand("sp_GetDisponibilidadesProfesor", CommandType.StoredProcedure);
                _connection.AssignParameter(true, "@cedula", cedula);
                _connection.OpenConnection();

                var result = _connection.ExecuteConsulta();
                disponibilidad.Cedula = cedula;
                while (result.Read())
                {
                    disponibilidad.IDDias.Add(Convert.ToInt32(result["id_dia"]));
                    disponibilidad.IDHoraInicio.Add(Convert.ToInt32(result["id_hora_inicio"]));
                    disponibilidad.IDHoraFin.Add(Convert.ToInt32(result["id_hora_fin"]));
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return disponibilidad;
            }
            finally
            {
                _connection.CloseConnection();
            }
            disponibilidad.HorasAsignadas = disponibilidad.IDHoraFin.Sum() - disponibilidad.IDHoraInicio.Sum();
            return disponibilidad;
        }
    }
}
