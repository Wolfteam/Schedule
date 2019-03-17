using Microsoft.EntityFrameworkCore;

namespace Schedule.API.Models.Repositories
{
    public class PrioridadesRepository
        : Repository<PrioridadProfesor>, IPrioridadesRepository
    {
        public HorariosContext HorariosContext
        {
            get { return _context as HorariosContext; }
        }

        public PrioridadesRepository(HorariosContext context)
            : base(context)
        {
        }
    }
}