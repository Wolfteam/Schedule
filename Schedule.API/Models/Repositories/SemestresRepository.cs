using System;
using System.Linq;
using AutoMapper.QueryableExtensions;
using Schedule.Entities;

namespace Schedule.API.Models.Repositories
{
    public class SemestresRepository : IRepository<Semestres, SemestreDTO>
    {
        private readonly HorariosContext _db = new HorariosContext();

        public bool Create(Semestres objeto)
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
        /// <summary>
        /// Obtiene todos los semestres
        /// </summary>
        /// <returns>IQueryable de semestres</returns>
        public IQueryable<SemestreDTO> Get()
        {
            return _db.Semestre.ProjectTo<SemestreDTO>();
        }

        public SemestreDTO Get(int id)
        {
            throw new NotImplementedException();
        }

        public bool Update(int id, Semestres objeto)
        {
            throw new NotImplementedException();
        }

        public bool Update(Semestres objeto)
        {
            throw new NotImplementedException();
        }
    }
}