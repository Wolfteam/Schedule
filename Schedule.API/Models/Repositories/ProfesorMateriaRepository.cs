using AutoMapper.QueryableExtensions;
using Schedule.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Schedule.API.Models.Repositories
{
    public class ProfesorMateriaRepository : IRepository<ProfesoresMaterias, ProfesorMateriaDetailsDTO>
    {
        private readonly HorariosContext _db = new HorariosContext();

        /// <summary>
        /// Crea una nueva relacion entre profesor y materia
        /// </summary>
        /// <param name="pm">Objeto tipo ProfesoresMaterias</param>
        /// <returns>True en caso de exito</returns>
        public bool Create(ProfesoresMaterias pm)
        {
            try
            {
                _db.ProfesoresMaterias.Add(pm);
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        public bool Delete()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Borra una relacion especifica entre profesor y materia
        /// </summary>
        /// <param name="id">ID de la tabla</param>
        /// <returns>True en caso de exito</returns>
        public bool Delete(int id)
        {
            try
            {
                var pm = _db.ProfesoresMaterias.FirstOrDefault(x => x.Id == id);
                if (pm != null)
                {
                    _db.ProfesoresMaterias.Remove(pm);
                    _db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Obtiene una lista de Profesores x Materias
        /// </summary>
        /// <returns>Lista de Profesores x Materias</returns>
        public IEnumerable<ProfesorMateriaDetailsDTO> Get()
        {
            return _db.ProfesoresMaterias.ProjectTo<ProfesorMateriaDetailsDTO>();
        }

        /// <summary>
        /// Obtiene la informacion pertinente a una relacion Profesor x Materia 
        /// particular acorde al id suministrado
        /// </summary>
        /// <param name="id">Id de la relacion</param>
        /// <returns>Objeto ProfesorMateriaDetailsDTO</returns>
        public ProfesorMateriaDetailsDTO Get(int id)
        {
            return _db.ProfesoresMaterias.ProjectTo<ProfesorMateriaDetailsDTO>()
                .FirstOrDefault(pm => pm.Id == id);
        }

        /// <summary>
        /// Obtiene la informacion pertinente a una relacion Profesor x Materia 
        /// particular acorde a la cedula y codigo suministrado
        /// </summary>
        /// <param name="cedula">Cedula del profesor</param>
        /// <param name="codigo">Codigo de la materia</param>
        /// <returns>Objeto ProfesorMateriaDetailsDTO</returns>
        public ProfesorMateriaDetailsDTO Get(uint cedula, int codigo)
        {
            return _db.ProfesoresMaterias
                .Where(pm => pm.Cedula == cedula && pm.Codigo == codigo)
                .ProjectTo<ProfesorMateriaDetailsDTO>()
                .FirstOrDefault();
        }

        /// <summary>
        ///  Actualiza una relacion entre profesor y materia
        /// </summary>
        /// <param name="id">Id de la tabla</param>
        /// <param name="pm">Objeto de tipo ProfesoresMaterias</param>
        /// <returns>True en caso de exito</returns>
        public bool Update(int id, ProfesoresMaterias pm)
        {
            try
            {
                Delete(id);
                _db.Add(pm);
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        public bool Update(ProfesoresMaterias objeto)
        {
            throw new NotImplementedException();
        }
    }
}
