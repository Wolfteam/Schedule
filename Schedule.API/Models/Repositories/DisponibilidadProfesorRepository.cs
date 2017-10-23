using System;
using System.Linq;
using System.Diagnostics;
using Schedule.Entities;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace Schedule.API.Models.Repositories
{
    public class DisponibilidadProfesorRepository
    {
        private readonly HorariosContext _db = new HorariosContext();

        /// <summary>
        /// Crea una nueva disponibilidad a un profesor en particular
        /// </summary>
        /// <param name="disponibilidad">Objeto de tipo disponibilidad</param>
        /// <returns>True en caso de exito</returns>
        public bool Create(DisponibilidadProfesores objeto)
        {
            try
            {
                _db.DisponibilidadProfesores.Add(objeto);
                _db.SaveChanges();
            }
            catch (Exception)
            {
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
        public DisponibilidadProfesorDTO Get(int cedula)
        {
            var disponibilidades = _db.DisponibilidadProfesores.Include(p => p.Profesores.PrioridadProfesor).Where(d => d.Cedula == cedula);
            DisponibilidadProfesorDTO disponibilidad = new DisponibilidadProfesorDTO
            {
                Cedula = 1,
                IDDias = disponibilidades.Select(w => w.IdDia).ToList(),
                IDHoraFin = disponibilidades.Select(x => x.IdHoraFin).ToList(),
                IDHoraInicio = disponibilidades.Select(y => y.IdHoraInicio).ToList(),
                HorasAsignadas = disponibilidades.Sum(x => x.IdHoraFin) - disponibilidades.Sum(y => y.IdHoraInicio),
                HorasACumplir = disponibilidades.FirstOrDefault().Profesores.PrioridadProfesor.HorasACumplir
            };
            return disponibilidad;
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
