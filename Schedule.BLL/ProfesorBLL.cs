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
            return _profesorDAO.Get(cedula);
        }

        public List<Profesor> GetAll()
        {
            return _profesorDAO.GetAll();
        }

        public bool Update(int cedula, Profesor profesor)
        {
            return _profesorDAO.Update(cedula, profesor);
        }
    }
}
