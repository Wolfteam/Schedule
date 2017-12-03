using Schedule.API.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Schedule.Entities;
using AutoMapper.QueryableExtensions;

namespace Schedule.API.Models.Repositories
{
    public class CarrerasRepository : IRepository<Carreras,CarreraDTO>
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

        /// <summary>
        /// Obtiene todas las carreras
        /// </summary>
        /// <returns>IQueryable de carreras</returns>
        public IEnumerable<CarreraDTO> Get()
        {
            return _db.Carreras.ProjectTo<CarreraDTO>();
        }

        public CarreraDTO Get(int id)
        {
            throw new NotImplementedException();
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
