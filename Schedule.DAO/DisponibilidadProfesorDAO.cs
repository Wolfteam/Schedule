using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Diagnostics;
using Schedule.Entities;
using System.Collections.Generic;

namespace Schedule.DAO
{
    public class DisponibilidadProfesorDAO : ICommon<DisponibilidadProfesor>
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
        public bool Create(DisponibilidadProfesor disponibilidad)
        {
            bool result = false;
            try
            {
                _connection.OpenConnection();
                _connection.CreateCommand("sp_CreateDisponibilidadProfesor", CommandType.StoredProcedure);
                _connection.AssignParameter(true, "@cedula", disponibilidad.Cedula);
                _connection.AssignParameter(true, "@idDia", disponibilidad.Dia.ID);
                _connection.AssignParameter(true, "@idHoraInicio", disponibilidad.HoraInicio.ID);
                _connection.AssignParameter(true, "@idHoraFin", disponibilidad.HoraFin.ID);
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
        /// Borra todas las disponibilidades de un profesor
        /// </summary>
        /// <param name="cedula">Cedula del profesor</param>
        /// <returns>True en caso de exito</returns>
        public bool Delete(int cedula)
        {
            bool result = false;
            try
            {
                _connection.OpenConnection();
                _connection.CreateCommand("sp_DeleteDisponibilidadProfesor", CommandType.StoredProcedure);
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
        /// Borra todas las disponibilidades
        /// </summary>
        /// <returns>True en caso de exito</returns>
        public bool Delete()
        {
            bool result = false;
            try
            {
                _connection.OpenConnection();
                _connection.CreateCommand("sp_DeleteDisponibilidadProfesor", CommandType.StoredProcedure);
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

        public DisponibilidadProfesor Get(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtiene una lista con todas las disponibilidades de los profesores
        /// </summary>
        /// <returns>Lista de disponibilidades</returns>
        public List<DisponibilidadProfesor> GetAll()
        {
            List<DisponibilidadProfesor> listaDisponibilidad = new List<DisponibilidadProfesor>();
            try
            {
                _connection.OpenConnection();
                _connection.CreateCommand("sp_GetDisponibilidadesProfesor", CommandType.StoredProcedure);

                var result = _connection.ExecuteConsulta();

                while (result.Read())
                {
                    DisponibilidadProfesor disponibilidad = new DisponibilidadProfesor
                    {
                        Cedula = Convert.ToInt32(result["cedula"]),
                        Dia = new DiasDAO().Get(Convert.ToInt32(result["id_dia"])),
                        HoraInicio = new HorasDAO().Get(Convert.ToInt32(result["id_hora_inicio"])),
                        HoraFin = new HorasDAO().Get(Convert.ToInt32(result["id_hora_fin"]))
                    };
                    listaDisponibilidad.Add(disponibilidad);
                }
            }
            catch (Exception)
            {
                return listaDisponibilidad;
            }
            finally
            {
                if (_connection != null) _connection.CloseConnection();
            }
            return listaDisponibilidad;
        }

        public bool Update(List<DisponibilidadProfesor> objeto)
        {
            throw new NotImplementedException();
        }

        public bool Update(DisponibilidadProfesor objeto)
        {
            throw new NotImplementedException();
        }

        public bool Update(int id, DisponibilidadProfesor objeto)
        {
            throw new NotImplementedException();
        }
    }
}
