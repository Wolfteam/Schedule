using System.Collections.Generic;
using Schedule.DAO;
using Schedule.Entities;

namespace Schedule.BLL
{

    public class ProfesorMateriaBLL
    {
        private ProfesorMateriaDAO _profesorMateriaDAO = null;
        public ProfesorMateriaBLL()
        {
            if (_profesorMateriaDAO == null) _profesorMateriaDAO = new ProfesorMateriaDAO();
        }

        public bool Create(ProfesorMateria pm)
        {
            return _profesorMateriaDAO.Create(pm.Profesor.Cedula, pm.Materia.Codigo);
        }

        public bool Delete(ProfesorMateria pm)
        {
            return _profesorMateriaDAO.Delete(pm.Profesor.Cedula, pm.Materia.Codigo);
        }

        public List<ProfesorMateria> GetAll()
        {
            return _profesorMateriaDAO.GetAll();
        }
    }
}
