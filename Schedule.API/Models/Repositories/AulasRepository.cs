using AutoMapper.QueryableExtensions;
using LinqKit;
using Schedule.Entities;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using AutoMapper;

namespace Schedule.API.Models.Repositories
{
    public class AulasRepository : Repository<Aulas>, IAulasRepository
    {
        private readonly IMapper _mapper;
        public HorariosContext HorariosContext
        {
            get { return _context as HorariosContext; }
        }

        public AulasRepository(IMapper mapper, HorariosContext context)
            : base(context)
        {
            _mapper = mapper;
        }

        #region Metodos de prueba con Datatables server side el incoveniente yace en la parte del sort debido a que quizas pueda implementarlo mejor

        public List<AulasDetailsDTO> GetTest(string searchBy, int take, int skip, string sortBy, bool sortDir, out int filteredResultsCount, out int totalResultsCount)
        {
            var whereClause = BuildDynamicWhereClause(searchBy);

            var result = HorariosContext.Aulas
                .AsExpandable()
                .ProjectTo<AulasDetailsDTO>(_mapper.ConfigurationProvider)
                .Where(whereClause);

            switch (sortBy.ToLower())
            {
                case "nombreaula":
                    result = sortDir ? result.OrderBy(x => x.NombreAula) : result.OrderByDescending(x => x.NombreAula);
                    break;
                case "capacidad":
                    result = sortDir ? result.OrderBy(x => x.Capacidad) : result.OrderByDescending(x => x.Capacidad);
                    break;
                case "tipoaula.nombretipo":
                    result = sortDir ? result.OrderBy(x => x.TipoAula.NombreTipo) : result.OrderByDescending(x => x.TipoAula.NombreTipo);
                    break;
                default://IdAula
                    result = sortDir ? result.OrderBy(x => x.IdAula) : result.OrderByDescending(x => x.IdAula);
                    break;
            }
            result = result.Skip(skip).Take(take);

            filteredResultsCount = HorariosContext.Aulas.AsExpandable().ProjectTo<AulasDetailsDTO>(_mapper.ConfigurationProvider).Where(whereClause).Count();
            totalResultsCount = HorariosContext.Aulas.Count();

            return result.ToList();
        }

        private Expression<Func<AulasDetailsDTO, bool>> BuildDynamicWhereClause(string searchValue)
        {
            // simple method to dynamically plugin a where clause
            var predicate = PredicateBuilder.New<AulasDetailsDTO>(true); // true -where(true) return all
            if (!String.IsNullOrWhiteSpace(searchValue))
            {
                List<string> searchTerms = searchValue.Split(' ').ToList().ConvertAll(x => x.ToLower());
                List<byte> search = new List<byte>();
                byte.TryParse(searchValue, out byte y);
                search.Add(y);
                predicate = predicate.Or(s => searchTerms.Any(srch => s.NombreAula.ToLower().Contains(srch)))
                                    .Or(s => search.Contains(s.Capacidad))
                                    .Or(s => searchTerms.Any(srch => s.TipoAula.NombreTipo.ToLower().Contains(srch)));
            }
            return predicate;
        }
        #endregion

        /// <summary>
        /// Obtiene todas las aulas que pertenecen a un tipo en particular y tienen una
        /// capacidad mayor o igual a la suministrada
        /// </summary>
        /// <param name="idTipo">Tipo de aula</param>
        /// <param name="capacidad">Capacidad del aula</param>
        /// <returns>IEnumerable de AulasDTO</returns>
        public IEnumerable<AulasDTO> GetByTipoCapacidad(byte idTipo, byte capacidad)
        {
            return HorariosContext.Aulas.ProjectTo<AulasDTO>(_mapper.ConfigurationProvider)
                .Where(au => au.IdTipo == idTipo && au.Capacidad >= capacidad)
                .OrderBy(au => au.Capacidad)
                .ToList();
        }
    }
}
