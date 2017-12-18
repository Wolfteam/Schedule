using Microsoft.EntityFrameworkCore;

namespace Schedule.API.Models.Repositories
{
    public class SemestresRepository : Repository<Semestres>, ISemestresRepository
    {
        public HorariosContext HorariosContext
        {
            get { return _context as HorariosContext; }
        }

        public SemestresRepository(DbContext context)
            : base(context)
        {
        }
    }
}