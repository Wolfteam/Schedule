using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Schedule.Entities;
using System.Linq;

namespace Schedule.API.Models.Repositories
{
    public class ProfesorMateriaRepository
        : Repository<ProfesoresMaterias>, IProfesorMateriaRepository
    {
        public HorariosContext HorariosContext
        {
            get { return _context as HorariosContext; }
        }

        public ProfesorMateriaRepository(DbContext context)
            : base(context)
        {
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
            return HorariosContext.ProfesoresMaterias
                .Where(pm => pm.Cedula == cedula && pm.Codigo == codigo)
                .ProjectTo<ProfesorMateriaDetailsDTO>()
                .FirstOrDefault();
        }
    }
}
