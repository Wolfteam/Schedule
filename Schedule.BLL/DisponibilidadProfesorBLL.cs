using System.Collections.Generic;
using Schedule.DAO;
using Schedule.Entities;

namespace Schedule.BLL
{
    public class DisponibilidadProfesorBLL
    {
        private DisponibilidadProfesorDAO _disponibilidadDAO = null;
        public DisponibilidadProfesorBLL()
        {
            if (_disponibilidadDAO == null) _disponibilidadDAO = new DisponibilidadProfesorDAO();
        }

        public bool Create(DisponibilidadProfesor disponibilidad)
        {
            for (int i = 0; i < disponibilidad.IDHoraInicio.Count; i++)
            {
                bool result = _disponibilidadDAO.Create(disponibilidad.Cedula,
                                disponibilidad.IDDias[i], disponibilidad.IDHoraInicio[i], 
                                disponibilidad.IDHoraFin[i]);
                if (!result) return false;
            }
            return true;
        }

        public bool Delete(int cedula)
        {
            return _disponibilidadDAO.Delete(cedula);
        }

        public bool Delete()
        {
            return _disponibilidadDAO.Delete();
        }

        public DisponibilidadProfesor Get(int cedula)
        {
            DisponibilidadProfesor disponibilidad = _disponibilidadDAO.Get(cedula);
            disponibilidad.HorasACumplir = new ProfesorDAO().GetPrioridad(cedula).HorasACumplir;
            return disponibilidad;
        }
    }
}
