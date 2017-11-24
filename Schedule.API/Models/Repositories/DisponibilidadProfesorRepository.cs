using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Schedule.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Schedule.API.Models.Repositories
{
    public class DisponibilidadProfesorRepository
    {
        private readonly HorariosContext _db = new HorariosContext();

        /// <summary>
        /// Crea una nueva disponibilidad a un profesor en particular
        /// </summary>
        /// <param name="disponibilidad">Objeto de tipo DisponibilidadProfesores</param>
        /// <returns>True en caso de exito</returns>
        public bool Create(IEnumerable<DisponibilidadProfesores> disponibilidad)
        {
            try
            {
                _db.DisponibilidadProfesores.AddRange(disponibilidad);
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine("error" + e.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Borra todas las disponibilidades
        /// </summary>
        /// <returns>True en caso de exito</returns>
        public bool Delete()
        {
            //TODO: Pensar una mejor solucion
            //Ten en cuenta que un borrado de mas de 1000 rows mataria a IIS
            // y al servidor de la base de datos
            try
            {
                //_db.DisponibilidadProfesores.RemoveRange(Get().ProjectTo<DisponibilidadProfesores>());
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Borra todas las disponibilidades de un profesor
        /// </summary>
        /// <param name="cedula">Cedula del profesor</param>
        /// <returns>True en caso de exito</returns>
        public bool Delete(int cedula)
        {
            try
            {
                _db.DisponibilidadProfesores
                .RemoveRange
                (
                    _db.DisponibilidadProfesores
                    .Where(x => x.Cedula == cedula)
                    .ToList()
                );
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        //public IQueryable<DisponibilidadProfesorDTO> Get()
        //{
        //    return _db.DisponibilidadProfesores.ProjectTo<DisponibilidadProfesorDTO>().OrderBy(x => x.Cedula);
        //}

        /// <summary>
        /// Obtiene una lista con todas las disponibilidad del profesor indicado
        /// </summary>
        /// <param name="cedula">Cedula del profesor</param>
        /// <returns>Lista de disponibilidades</returns>
        public DisponibilidadProfesorDetailsDTO Get(int cedula)
        {

            var disponibilidad = _db.DisponibilidadProfesores.Include(p => p.Profesores.PrioridadProfesor).Where(c => c.Cedula == cedula);
            if (disponibilidad.Count() == 0)
                return new DisponibilidadProfesorDetailsDTO();

            DisponibilidadProfesorDetailsDTO result = new DisponibilidadProfesorDetailsDTO
            {
                Cedula = (uint)cedula,
                Disponibilidad = disponibilidad.ProjectTo<DisponibilidadProfesorDTO>(),
                HorasACumplir = disponibilidad.FirstOrDefault().Profesores.PrioridadProfesor.HorasACumplir,
                HorasAsignadas = disponibilidad.Sum(hf => hf.IdHoraFin) - disponibilidad.Sum(hi => hi.IdHoraInicio)
            };
            return result;
        }

        public bool Update(int cedula, DisponibilidadProfesores objeto)
        {
            throw new NotImplementedException();
        }

        public bool Update(DisponibilidadProfesores objeto)
        {
            throw new NotImplementedException();
        }
    }
}
