using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Schedule.Entities;
using System.Linq;
using System.Collections.Generic;

namespace Schedule.API.Models.Repositories
{
    public class MateriasRepository : Repository<Materias>, IMateriasRepository
    {

        public HorariosContext HorariosContext
        {
            get { return _context as HorariosContext; }
        }

        public MateriasRepository(DbContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Obtiene todas las materias para un semestre en particular
        /// </summary>
        /// <param name="idSemestre">Id del semestre</param>
        /// <returns>IEnumerable de MateriasDTO</returns>
        public IEnumerable<MateriasDTO> GetBySemestre(int idSemestre)
        {
            return HorariosContext.Materias.ProjectTo<MateriasDTO>().Where(m => m.IdSemestre == idSemestre);
        }
    }
}
