using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Schedule.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Schedule.API.Models.Repositories
{
    public class HorarioProfesorRepository //: IRepository<Aulas, AulasDetailsDTO>
    {
        private readonly HorariosContext _db = new HorariosContext();

        /// <summary>
        /// Guarda una lista de horarios de un profesor
        /// </summary>
        /// <param name="horarios">IEnumerable de HorarioProfesores</param>
        /// <returns>True en caso de exito</returns>
        public bool Create(IEnumerable<HorarioProfesores> horarios)
        {
            try
            {
                _db.HorarioProfesores.AddRange(horarios);
                _db.SaveChanges();
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Obtiene una lista de los horarios de un profesor en un dia en particular
        /// </summary>
        /// <param name="cedula">Cedula del profesor</param>
        /// <param name="idDia">Id del dia</param>
        /// <returns>IEnumerable de HorarioProfesorDTO</returns>
        public IEnumerable<HorarioProfesorDTO> GetByCedulaDia(uint cedula, byte idDia)
        {
            return _db.HorarioProfesores
                .ProjectTo<HorarioProfesorDTO>()
                .Where(hp => hp.Cedula == cedula && hp.IdDia == idDia);
        }

        /// <summary>
        /// Obtiene una lista de los horarios de los profesores por dia y aula
        /// </summary>
        /// <param name="idDia">Id del dia</param>
        /// <param name="idAula">Id del aula</param>
        /// <returns>IEnumerable de HorarioProfesorDTO</returns>
        public IEnumerable<HorarioProfesorDTO> GetByDiaAula(byte idDia, byte idAula)
        {
            return _db.HorarioProfesores
                .Include(pc => pc.PeriodoCarrera)
                .Where(hp => hp.PeriodoCarrera.Status == true && hp.IdDia == idDia && hp.IdAula == idAula)
                .ProjectTo<HorarioProfesorDTO>();
        }

        /// <summary>
        /// Obtiene una lista de los horarios de los profesores por semestre y dia
        /// </summary>
        /// <param name="idSemestre">Id del semestre</param>
        /// <param name="idDia">Id del dia</param>
        /// <returns>IEnumerable de HorarioProfesorDTO</returns>
        public IEnumerable<HorarioProfesorDTO> GetBySemestreDia(byte idSemestre, byte idDia)
        {
            return _db.HorarioProfesores
                .Include(pc => pc.PeriodoCarrera)
                //.Where(hp => hp.PeriodoCarrera.Status == true && hp.IdDia == idDia)
                .Join
                (
                    _db.Materias, //target
                    hp => hp.Codigo, //fk
                    m => m.Codigo, //pk
                    (hp, m) => new { Materias = m, HorarioProfesor = hp } //select result
                )
                .Where
                (
                    x => x.Materias.IdSemestre == idSemestre 
                    && x.HorarioProfesor.PeriodoCarrera.Status == true 
                    && x.HorarioProfesor.IdDia ==  idDia
                )
                .Select(hp => hp.HorarioProfesor)
                .ProjectTo<HorarioProfesorDTO>();
        }

    }
}