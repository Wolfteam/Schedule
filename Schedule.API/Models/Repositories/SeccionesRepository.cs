using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Schedule.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Schedule.API.Models.Repositories
{
    public class SeccionesRepository : Repository<Secciones>, ISeccionesRepository
    {
        public HorariosContext HorariosContext
        {
            get { return _context as HorariosContext; }
        }
        
        public SeccionesRepository(DbContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Obtiene una lista de todas las secciones 
        /// pertenicientes al periodo actual
        /// </summary>
        /// <returns>Lista de secciones</returns>
        public IEnumerable<SeccionesDetailsDTO> GetAllCurrent()
        {
            return HorariosContext.Secciones
                .Where(x => x.PeriodoCarrera.Status == true)
                .ProjectTo<SeccionesDetailsDTO>();
        }

        /// <summary>
        /// Obtiene una seccion en particular perteniciente al periodo actual
        /// </summary>
        /// <param name="codigo">Id de la materia a buscar</param>
        /// <returns>Objeto Secciones</returns>
        public SeccionesDetailsDTO GetCurrent(int codigo)
        {
            return HorariosContext.Secciones
                .Where(x => x.PeriodoCarrera.Status == true)
                .ProjectTo<SeccionesDetailsDTO>()
                .FirstOrDefault(x => x.Materia.Codigo == codigo);
        }
    }
}
