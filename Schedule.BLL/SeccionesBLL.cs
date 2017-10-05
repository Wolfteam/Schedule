using System.Collections.Generic;
using Schedule.DAO;
using Schedule.Entities;

namespace Schedule.BLL
{
    public class SeccionesBLL
    {
        private SeccionesDAO _seccionesDAO = null;

        public SeccionesBLL()
        {
            if (_seccionesDAO == null) _seccionesDAO = new SeccionesDAO();
        }

        public bool Create(Secciones secciones)
        {
            return _seccionesDAO.Create(secciones);
        }

        public bool Delete(int codigo)
        {
            return _seccionesDAO.Delete(codigo);
        }

        public bool Delete()
        {
            return _seccionesDAO.Delete();
        }

        public Secciones Get(int codigo)
        {
            return _seccionesDAO.Get(codigo);
        }

        public List<Secciones> GetAll()
        {
            return _seccionesDAO.GetAll();
        }

        public bool Update(int codigo, Secciones seccion)
        {
            return _seccionesDAO.Update(codigo, seccion);
        }
    }
}
