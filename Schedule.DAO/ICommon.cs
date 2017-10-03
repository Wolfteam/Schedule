using System.Collections.Generic;

namespace Schedule.DAO
{
    interface ICommon<T>
    {
        bool Create(T objeto);
        T Get(int id);
        List<T> GetAll();
        bool Delete(int id);
        bool Delete();
        bool Update(T objeto);
    }
}
