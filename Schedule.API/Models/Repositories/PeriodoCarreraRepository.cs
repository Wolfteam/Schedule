using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Schedule.Entities;
using System;
using System.Diagnostics;
using System.Linq;

namespace Schedule.API.Models.Repositories
{
    public class PeriodoCarreraRepository : //IRepository<PeriodoCarrera, PeriodoCarreraDTO>
    {
        private readonly HorariosContext _db = new HorariosContext();


        /// <summary>
        /// Crea una nueva periodo academico
        /// </summary>
        /// <param name="periodo">Objeto de tipo PeriodoCarrera</param>
        /// <returns>True en caso de exito</returns>
        public bool Create(PeriodoCarrera periodo)
        {
            try
            {
                _db.Materias.Add(materia);
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Borra un periodo academico especifico
        /// </summary>
        /// <param name="id">Id del periodo a eliminar</param>
        /// <returns>True en caso de exito</returns>
        public bool Delete(int id)
        {
            try
            {
                var periodo = _db.PeriodoCarrera.FirstOrDefault(x => x.IdPeriodo == id);
                if (periodo != null)
                {
                    _db.PeriodoCarrera.Remove(periodo);
                    _db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Obtiene todos los periodos academicos creados
        /// </summary>
        /// <returns>Lista de materias</returns>
        public IQueryable<PeriodoCarreraDTO> Get()
        {
            return _db.PeriodoCarrera.ProjectTo<PeriodoCarreraDTO>();
        }

        /// <summary>
        /// Obtiene el periodo actual el cual tiene el status activo
        /// </summary>
        /// <returns>Objeto PeriodoCarrera</returns>
        public PeriodoCarreraDTO GetCurrentPeriodo()
        {
            return _db.PeriodoCarrera.ProjectTo<PeriodoCarreraDTO>().FirstOrDefault(x => x.Status == true);
        }


    }
}