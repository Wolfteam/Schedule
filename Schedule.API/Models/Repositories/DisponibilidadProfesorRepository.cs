﻿using AutoMapper.QueryableExtensions;
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
                Delete(disponibilidad.FirstOrDefault().Cedula);
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
        public bool Delete(uint cedula)
        {
            try
            {
                _db.DisponibilidadProfesores
                .RemoveRange
                (
                    _db.DisponibilidadProfesores
                    .Include(pc => pc.PeriodoCarrera)
                    .Where(x => x.Cedula == cedula && x.PeriodoCarrera.Status == true)
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

        /// <summary>
        /// Obtiene una lista de todas las disponibilidades de los profesores
        /// </summary>
        /// <returns>IEnumerable de DisponibilidadProfesorDTO</returns>
        public IEnumerable<DisponibilidadProfesorDetailsDTO> Get()
        {
            var disponibilidades = _db.DisponibilidadProfesores
                 .Include(pc => pc.PeriodoCarrera)
                 .Include(p => p.Profesores.PrioridadProfesor)
                 .Where(c => c.PeriodoCarrera.Status == true);

            List<DisponibilidadProfesorDetailsDTO> result = new List<DisponibilidadProfesorDetailsDTO>();
            foreach (uint cedula in disponibilidades.Select(d => d.Cedula).Distinct())
            {
                var disp = disponibilidades.Where(d => d.Cedula == cedula);
                DisponibilidadProfesorDetailsDTO dto = new DisponibilidadProfesorDetailsDTO
                {
                    Cedula = cedula,
                    Disponibilidad = disp.ProjectTo<DisponibilidadProfesorDTO>(),
                    HorasACumplir = disp.FirstOrDefault(d => d.Cedula == cedula).Profesores.PrioridadProfesor.HorasACumplir,
                    HorasAsignadas = (byte)(disp.Sum(hf => hf.IdHoraFin) - disp.Sum(hi => hi.IdHoraInicio))
                };
                result.Add(dto);
            }
            return result;
        }

        /// <summary>
        /// Obtiene una lista con todas las disponibilidad del profesor indicado
        /// </summary>
        /// <param name="cedula">Cedula del profesor</param>
        /// <returns>Lista de disponibilidades</returns>
        public DisponibilidadProfesorDetailsDTO Get(int cedula)
        {
            var disponibilidad = _db.DisponibilidadProfesores
                .Include(pc => pc.PeriodoCarrera)
                .Include(p => p.Profesores.PrioridadProfesor)
                .Where(c => c.Cedula == cedula && c.PeriodoCarrera.Status == true);

            if (disponibilidad.Count() == 0)
                return new DisponibilidadProfesorDetailsDTO();

            DisponibilidadProfesorDetailsDTO result = new DisponibilidadProfesorDetailsDTO
            {
                Cedula = (uint)cedula,
                Disponibilidad = disponibilidad.ProjectTo<DisponibilidadProfesorDTO>(),
                HorasACumplir = disponibilidad.FirstOrDefault().Profesores.PrioridadProfesor.HorasACumplir,
                HorasAsignadas = (byte)(disponibilidad.Sum(hf => hf.IdHoraFin) - disponibilidad.Sum(hi => hi.IdHoraInicio))
            };
            return result;
        }

        //TODO: Quitar el remapeo de los metodos que lo usan y moverlo a una funcion
        /// <summary>
        /// Obtiene la disponibilidades de los profesores acorde a una prioridad 
        /// y una materia
        /// </summary>
        /// <param name="idPrioridad">Id de la prioridad</param>
        /// <param name="codigo">Codigo de la materia</param>
        /// <returns>IEnumerable de DisponibilidadProfesorDetailsDTO</returns>
        public IEnumerable<DisponibilidadProfesorDetailsDTO> GetByPrioridadMateria(byte idPrioridad, ushort codigo)
        {
            var disponibilidades = _db.DisponibilidadProfesores
                 .Include(pc => pc.PeriodoCarrera)
                 .Include(pm => pm.Profesores.ProfesoresMaterias)
                 .Include(p => p.Profesores.PrioridadProfesor)
                 .Where
                 (
                     c => c.PeriodoCarrera.Status == true &&
                     c.Profesores.PrioridadProfesor.IdPrioridad == idPrioridad && 
                     c.Profesores.ProfesoresMaterias.FirstOrDefault(pm => pm.Codigo == codigo) != null
                 );

            List<DisponibilidadProfesorDetailsDTO> result = new List<DisponibilidadProfesorDetailsDTO>();
            foreach (uint cedula in disponibilidades.Select(d => d.Cedula).Distinct())
            {
                var disp = disponibilidades.Where(d => d.Cedula == cedula);
                DisponibilidadProfesorDetailsDTO dto = new DisponibilidadProfesorDetailsDTO
                {
                    Cedula = cedula,
                    Disponibilidad = disp.ProjectTo<DisponibilidadProfesorDTO>(),
                    HorasACumplir = disp.FirstOrDefault(d => d.Cedula == cedula).Profesores.PrioridadProfesor.HorasACumplir,
                    HorasAsignadas = (byte)(disp.Sum(hf => hf.IdHoraFin) - disp.Sum(hi => hi.IdHoraInicio))
                };
                result.Add(dto);
            }
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
