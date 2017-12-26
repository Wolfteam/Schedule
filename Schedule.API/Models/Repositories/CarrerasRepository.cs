using Microsoft.EntityFrameworkCore;

namespace Schedule.API.Models.Repositories
{
    public class CarrerasRepository : Repository<Carreras>, ICarrerasRepository
    {
        public HorariosContext HorariosContext
        {
            get
            {
                return _context as HorariosContext;
            }
        }

        public CarrerasRepository(DbContext context)
            : base(context)
        {
        }
    }
}