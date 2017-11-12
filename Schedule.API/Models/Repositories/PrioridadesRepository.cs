using System;
using System.Linq;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using System.Collections.Generic;
using Schedule.Entities;
using LinqKit;
using System.Linq.Expressions;

namespace Schedule.API.Models.Repositories
{
    public class PrioridadesRepository //: IRepository<Aulas, AulasDetailsDTO>
    {
        private readonly HorariosContext _db = new HorariosContext();

        /// <summary>
        /// Obtiene todos las prioridades
        /// </summary>
        /// <returns>IQueryable de semestres</returns>
        public IQueryable<PrioridadProfesorDTO> Get()
        {
            return _db.PrioridadProfesor.ProjectTo<PrioridadProfesorDTO>();
        }
    }
}
