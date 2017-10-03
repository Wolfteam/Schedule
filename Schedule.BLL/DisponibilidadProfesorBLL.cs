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

        public bool Create(List<DisponibilidadProfesor> disponibilidades)
        {
            foreach (var disponibilidad in disponibilidades)
            {
                bool result = _disponibilidadDAO.Create(disponibilidad);
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

        public List<DisponibilidadProfesor> GetAll()
        {
            return _disponibilidadDAO.GetAll();
        }
    }
}
