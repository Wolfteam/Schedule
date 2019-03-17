using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Schedule.Entities;
using System.Linq;

namespace Schedule.API.Models.Repositories
{
    public class PeriodoCarreraRepository 
        : Repository<PeriodoCarrera>, IPeriodoCarreraRepository
    {
        private readonly IMapper _mapper;

        public HorariosContext HorariosContext
        {
            get { return _context as HorariosContext; }
        }

        public PeriodoCarreraRepository(HorariosContext context, IMapper mapper) 
            : base(context)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Obtiene el periodo actual el cual tiene el status activo
        /// </summary>
        /// <returns>Objeto PeriodoCarrera</returns>
        public PeriodoCarreraDTO GetCurrentPeriodo()
        {
            return HorariosContext.PeriodoCarrera
                .ProjectTo<PeriodoCarreraDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefault(x => x.Status == true);
        }


        /// <summary>
        /// Actualiza el status de todos los periodos que se encuentren con
        /// status activo a inactivo
        /// </summary>
        public void UpdateAllCurrentStatus()
        {
            var periodos = HorariosContext.PeriodoCarrera.Where(pe => pe.Status == true);
            if (periodos == null)
                return;
            foreach (var periodo in periodos)
            {
                periodo.Status = false;
            }
            HorariosContext.PeriodoCarrera.UpdateRange(periodos);
            //HorariosContext.SaveChanges();
        }
    }
}