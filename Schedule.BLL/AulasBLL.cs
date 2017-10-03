using System.Collections.Generic;
using Schedule.DAO;
using Schedule.Entities;

namespace Schedule.BLL
{

    public class AulasBLL
    {
        private AulasDAO _aulasDAO = null;
        public AulasBLL()
        {
            if (_aulasDAO == null) _aulasDAO = new AulasDAO();
        }

        public bool Create(Aulas aula)
        {
            return _aulasDAO.Create(aula);
        }

        public bool Delete (int id)
        {
            return _aulasDAO.Delete(id);
        }

        public List<Aulas> GetAll()
        {
            return _aulasDAO.GetAll();
        }
        public Aulas Get(int id)
        {
            return _aulasDAO.Get(id);
        }
    }
}
