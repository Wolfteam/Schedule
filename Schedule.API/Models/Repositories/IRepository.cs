using System.Linq;

namespace Schedule.API.Models.Repositories
{
    public interface IRepository<T,Y>
    {
        IQueryable<Y> Get();
        Y Get(int id);
        bool Create(T objeto);
        bool Delete();
        bool Delete(int id);
        bool Update(int id, T objeto);
        bool Update(T objeto);
    }
}
