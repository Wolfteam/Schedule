using System.Collections.Generic;
using Schedule.DAO;
using Schedule.Entities;

namespace Schedule.BLL
{

    public class MateriasBLL
    {
        private MateriasDAO _materiaDAO = null;
        public MateriasBLL()
        {
            _materiaDAO = new MateriasDAO();
        }

        public bool Create (Materias materia)
        {
            return _materiaDAO.Create(materia);
        }

        public bool Delete(int codigo)
        {
            return _materiaDAO.Delete(codigo);
        }

        public List<Materias> GetAll()
        {
            return _materiaDAO.GetAll();
        }
        public Materias Get(int codigo)
        {
            return _materiaDAO.Get(codigo);
        }
    }
}