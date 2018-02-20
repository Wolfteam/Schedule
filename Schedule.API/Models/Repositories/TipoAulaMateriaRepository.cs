using Microsoft.EntityFrameworkCore;

namespace Schedule.API.Models.Repositories
{
    public class TipoAulaMateriaRepository : Repository<TipoAulaMaterias>, ITipoAulaMateriaRepository
    {
        public HorariosContext HorariosContext
        {
            get { return _context as HorariosContext; }
        }

        public TipoAulaMateriaRepository(DbContext context)
            : base(context)
        {
        }
    }
}