using Microsoft.EntityFrameworkCore;
using Schedule.Entities;
using System.Linq;

namespace Schedule.API.Models.Repositories
{
    public class ProfesorRepository : Repository<Profesores>, IProfesorRepository
    {
        public HorariosContext HorariosContext
        {
            get { return _context as HorariosContext; }
        }

        public ProfesorRepository(DbContext context) 
            : base(context)
        {
        }

        /// <summary>
        /// Obtiene las horas a cumplir de un profesor en particular
        /// </summary>
        /// <param name="cedula">Cedula</param>
        /// <returns>Numero de horas que un profesor debe cumplir</returns>
        public byte GetHorasACumplir(int cedula)
        {
            return HorariosContext.Profesores
                .Include(x => x.PrioridadProfesor)
                .FirstOrDefault(x => x.Cedula == cedula)
                .PrioridadProfesor.HorasACumplir;
        }
    }
}
