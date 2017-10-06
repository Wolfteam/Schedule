using System.Collections.Generic;
using Schedule.DAO;
using Schedule.Entities;

namespace Schedule.BLL
{

    public class ProfesorBLL
    {
        private ProfesorDAO _profesorDAO = null;
        public ProfesorBLL()
        {
            if (_profesorDAO == null) _profesorDAO = new ProfesorDAO();
        }

        public bool Create (Profesor profesor)
        {
            return _profesorDAO.Create(profesor);
        }

        public bool Delete (int cedula) 
        {
            return _profesorDAO.Delete(cedula);
        }

        public Profesor Get(int cedula)
        {
            Profesor profesor = _profesorDAO.Get(cedula);
            profesor.Prioridad = _profesorDAO.GetPrioridad(profesor.Cedula);
            return profesor;
        }

        public List<Profesor> GetAll()
        {
            List<Profesor> listaProfesores = _profesorDAO.GetAll();
            foreach (var profesor in listaProfesores)
            {
                profesor.Prioridad = _profesorDAO.GetPrioridad(profesor.Cedula);
            }
            return listaProfesores;
        }

        public bool Update(int cedula, Profesor profesor)
        {
            return _profesorDAO.Update(cedula, profesor);
        }
    }
}
