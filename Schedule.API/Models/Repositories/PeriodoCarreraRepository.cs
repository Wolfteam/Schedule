using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Schedule.Entities;
using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;

namespace Schedule.API.Models.Repositories
{
    public class PeriodoCarreraRepository : IRepository<PeriodoCarrera, PeriodoCarreraDTO>
    {
        private readonly HorariosContext _db = new HorariosContext();

        /// <summary>
        /// Crea un nuevo periodo academico
        /// </summary>
        /// <param name="periodo">Objeto PeriodoCarrera </param>
        /// <returns>True en caso de exito</returns>
        public bool Create(PeriodoCarrera periodo)
        {
            bool result = false;
            try
            {
                _db.PeriodoCarrera.Add(periodo);
                _db.SaveChanges();
                result = true;
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return result;
        }

        public bool Delete()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Elimina un periodo academico en particular
        /// </summary>
        /// <param name="id">Id del periodo academico a borrar</param>
        /// <returns>True en caso de exito</returns>
        public bool Delete(int id)
        {
            bool result = true;
            try
            {
                var periodo = _db.PeriodoCarrera.FirstOrDefault(pe => pe.IdPeriodo == id);
                if (periodo != null)
                {
                    _db.PeriodoCarrera.Remove(periodo);
                    _db.SaveChanges();
                }
            }
            catch (System.Exception e)
            {
                result = false;
                Console.WriteLine(e.Message);
            }
            return result;
        }

        /// <summary>
        /// Obtiene una lista de todos los periodos academicos
        /// </summary>
        /// <returns>IQueryable de PeriodoCarreraDTO</returns>
        public IEnumerable<PeriodoCarreraDTO> Get()
        {
            return _db.PeriodoCarrera.ProjectTo<PeriodoCarreraDTO>();
        }

        /// <summary>
        /// Obtiene un periodo academico en particular
        /// </summary>
        /// <param name="id">Id del periodo academico</param>
        /// <returns>Objeto PeriodoCarreraDTO</returns>
        public PeriodoCarreraDTO Get(int id)
        {
            return _db.PeriodoCarrera
                .ProjectTo<PeriodoCarreraDTO>()
                .FirstOrDefault(pe => pe.IdPeriodo == id);
        }

        /// <summary>
        /// Obtiene el periodo actual el cual tiene el status activo
        /// </summary>
        /// <returns>Objeto PeriodoCarrera</returns>
        public PeriodoCarreraDTO GetCurrentPeriodo()
        {
            return _db.PeriodoCarrera
                .ProjectTo<PeriodoCarreraDTO>()
                .FirstOrDefault(x => x.Status == true);
        }

        public bool Update(int id, PeriodoCarrera periodo)
        {
            try
            {
                var pc = _db.PeriodoCarrera.FirstOrDefault(x => x.IdPeriodo == id);
                if (pc == null)
                    return false;
                
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Actualiza un periodo academico
        /// </summary>
        /// <param name="periodo">Objeto PeriodoCarrera</param>
        /// <returns>True en caso de exito</returns>
        public bool Update(PeriodoCarrera periodo)
        {
            try
            {
                var oldPeriodo = _db.PeriodoCarrera.FirstOrDefault(pc => pc.IdPeriodo == periodo.IdPeriodo);
                if  (oldPeriodo == null)
                    return false;
                _db.Entry(oldPeriodo).CurrentValues.SetValues(periodo);
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
            return true;
        }
            
        /// <summary>
        /// Actualiza el status de todos los periodos que se encuentren con
        /// status activo a inactivo
        /// </summary>
        public void UpdateAllCurrentStatus()
        {
            var periodos = _db.PeriodoCarrera.Where(pe => pe.Status == true);
            if (periodos == null)
                return;
            foreach (var periodo in periodos)
            {
                periodo.Status = false;
            }
            _db.PeriodoCarrera.UpdateRange(periodos);
            _db.SaveChanges();
        }
    }
}