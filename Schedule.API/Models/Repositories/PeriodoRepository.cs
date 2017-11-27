using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Schedule.Entities;
using System;
using System.Diagnostics;
using System.Linq;

namespace Schedule.API.Models.Repositories
{
    public class PeriodoRepository //: IRepository<Materias, MateriasDetailsDTO>
    {
        private readonly HorariosContext _db = new HorariosContext();

        /// <summary>
        /// Obtiene el periodo actual el cual tiene el status activo
        /// </summary>
        /// <returns>Objeto PeriodoCarrera</returns>
        public PeriodoCarrera GetCurrentPeriodo()
        {
            return _db.PeriodoCarrera.FirstOrDefault(x => x.Status == true);
        }
    }
}