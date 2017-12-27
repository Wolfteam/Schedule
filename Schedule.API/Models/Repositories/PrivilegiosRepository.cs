using Microsoft.EntityFrameworkCore;

namespace Schedule.API.Models.Repositories
{
    public class PrivilegiosRepository : Repository<Privilegios>, IPrivilegiosRepository
    {
        public HorariosContext HorariosContext
        {
            get { return _context as HorariosContext; }
        }

        public PrivilegiosRepository(DbContext context)
            : base(context)
        {
        }
    }
}