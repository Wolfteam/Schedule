using Schedule.API.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Schedule.API.Models.Repositories
{
    public class CarrerasRepository : IRepository<Carreras,Carreras>
    {
        private readonly HorariosContext _db = new HorariosContext();

        public bool Create(Carreras objeto)
        {
            throw new NotImplementedException();
        }

        public bool Delete()
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Carreras> Get()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtiene una carrera en particular
        /// </summary>
        /// <param name="id">ID de la carrera a buscar</param>
        /// <returns>Objeto Carrera</returns>
        public Carreras Get(int id)
        {
            return _db.Carreras.FirstOrDefault(x => x.IdCarrera == id);
        }

        public bool Update(int id, Carreras objeto)
        {
            throw new NotImplementedException();
        }

        public bool Update(Carreras objeto)
        {
            throw new NotImplementedException();
        }
    }
}
