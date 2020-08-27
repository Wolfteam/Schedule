using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Schedule.Entities;
using System.Linq;
using System.Collections.Generic;
using AutoMapper;

namespace Schedule.API.Models.Repositories
{
    public class MateriasRepository : Repository<Materias>, IMateriasRepository
    {
        private readonly IMapper _mapper;

        public HorariosContext HorariosContext
        {
            get { return _context as HorariosContext; }
        }

        public MateriasRepository(HorariosContext context, IMapper mapper)
            : base(context)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Obtiene todas las materias para un semestre en particular
        /// </summary>
        /// <param name="idSemestre">Id del semestre</param>
        /// <returns>IEnumerable de MateriasDTO</returns>
        public IEnumerable<MateriasDTO> GetBySemestre(int idSemestre)
        {
            return HorariosContext.Materias.ProjectTo<MateriasDTO>(_mapper.ConfigurationProvider).Where(m => m.IdSemestre == idSemestre).ToList();
        }
    }
}
