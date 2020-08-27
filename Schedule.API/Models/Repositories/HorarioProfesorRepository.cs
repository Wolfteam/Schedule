using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Schedule.API.Helpers;
using Schedule.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Schedule.API.Models.Repositories
{
    public class HorarioProfesorRepository 
        : Repository<HorarioProfesores>, IHorarioProfesorRepository
    {
        private readonly IMapper _mapper;

        public HorariosContext HorariosContext
        {
            get { return _context as HorariosContext; }
        }

        public HorarioProfesorRepository(HorariosContext context, IMapper mapper) 
            : base(context)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Calcula las horas asignadas para un profesor en particular.
        /// No confundir estas horas asignadas con las de disponibilidad.
        /// </summary>
        /// <param name="cedula">Cedula del profesor</param>
        /// <returns>Numero de horas en las que da clase</returns>
        public int CalculateHorasAsignadas(uint cedula)
        {
            return 
                base.Get(hp => hp.Cedula == cedula)
                .Select(d => d.IdHoraFin - d.IdHoraInicio)
                .Sum();
        }

        /// <summary>
        /// Verifica si existen registros existentes para el periodo academico
        /// acatual
        /// </summary>
        /// <returns>True en caso de existir registros</returns>
        public bool RecordsExists()
        {
            return base
                .Get(pc => pc.PeriodoCarrera.Status == true, null, "PeriodoCarrera")
                .Count() > 0 ? true : false;
        }

        /// <summary>
        /// Obtiene los horarios de los profesores del periodo academico actual
        /// para una aula en particular
        /// </summary>
        /// <param name="idAula">Id del aula</param>
        /// <returns>IEnumerable de HorarioProfesorDetailsDTO</returns>
        public IEnumerable<HorarioProfesorDetailsDTO> GetByAula(int idAula)
        {
            IEnumerable<HorarioProfesorDetailsDTO> lista = null;
            HorariosContext.LoadStoredProc("spGetHorariosProfesores")
                .WithSqlParam("@idSemestre", null)
                .WithSqlParam("@idAula", idAula)
                .ExecuteStoredProc((handler) =>
                {
                    lista = handler.ReadToList<HorarioProfesorDetailsDTO>();
                });
            return lista;
        }

        /// <summary>
        /// Obtiene una lista de los horarios de un profesor en un dia en particular
        /// </summary>
        /// <param name="cedula">Cedula del profesor</param>
        /// <param name="idDia">Id del dia</param>
        /// <returns>IEnumerable de HorarioProfesorDTO</returns>
        public IEnumerable<HorarioProfesorDTO> GetByCedulaDia(uint cedula, byte idDia)
        {
            return HorariosContext.HorarioProfesores
                .ProjectTo<HorarioProfesorDTO>(_mapper.ConfigurationProvider)
                .Where(hp => hp.Cedula == cedula && hp.IdDia == idDia)
                .ToList();
        }

        /// <summary>
        /// Obtiene una lista de los horarios de los profesores por dia y aula
        /// </summary>
        /// <param name="idDia">Id del dia</param>
        /// <param name="idAula">Id del aula</param>
        /// <returns>IEnumerable de HorarioProfesorDTO</returns>
        public IEnumerable<HorarioProfesorDTO> GetByDiaAula(byte idDia, byte idAula)
        {
            return HorariosContext.HorarioProfesores
                .Include(pc => pc.PeriodoCarrera)
                .Where(hp => hp.PeriodoCarrera.Status == true && hp.IdDia == idDia && hp.IdAula == idAula)
                .ProjectTo<HorarioProfesorDTO>(_mapper.ConfigurationProvider)
                .ToList();
        }

        /// <summary>
        /// Obtiene una lista de los horarios de los profesores por semestre y dia
        /// </summary>
        /// <param name="idSemestre">Id del semestre</param>
        /// <param name="idDia">Id del dia</param>
        /// <returns>IEnumerable de HorarioProfesorDTO</returns>
        public IEnumerable<HorarioProfesorDTO> GetBySemestreDia(byte idSemestre, byte idDia)
        {
            return HorariosContext.HorarioProfesores
                .Include(pc => pc.PeriodoCarrera)
                //.Where(hp => hp.PeriodoCarrera.Status == true && hp.IdDia == idDia)
                .Join
                (
                    HorariosContext.Materias, //target
                    hp => hp.Codigo, //fk
                    m => m.Codigo, //pk
                    (hp, m) => new { Materias = m, HorarioProfesor = hp } //select result
                )
                .Where
                (
                    x => x.Materias.IdSemestre == idSemestre
                    && x.HorarioProfesor.PeriodoCarrera.Status == true
                    && x.HorarioProfesor.IdDia == idDia
                )
                .Select(hp => hp.HorarioProfesor)
                .ProjectTo<HorarioProfesorDTO>(_mapper.ConfigurationProvider)
                .ToList();
        }

        /// <summary>
        /// Obtiene los horarios de los profesores del periodo academico actual
        /// para un semestre en particular
        /// </summary>
        /// <param name="idSemestre">Id del semestre</param>
        /// <returns>IEnumerable de HorarioProfesorDetailsDTO</returns>
        public IEnumerable<HorarioProfesorDetailsDTO> GetBySemestre(int idSemestre)
        {
            IEnumerable<HorarioProfesorDetailsDTO> lista = null;
            HorariosContext.LoadStoredProc("spGetHorariosProfesores")
                .WithSqlParam("@idSemestre", idSemestre)
                .WithSqlParam("@idAula", null)
                .ExecuteStoredProc((handler) =>
                {
                    lista = handler.ReadToList<HorarioProfesorDetailsDTO>();
                });
            return lista;
        }

        /// <summary>
        /// Obtienes la ultima seccion asignada a una materia en particular
        /// </summary>
        /// <param name="codigo">Codigo de la materia</param>
        /// <returns>Ultima seccion asignada a la materia</returns>
        public int GetLastSeccionAssigned(ushort codigo)
        {
            var horario = HorariosContext.HorarioProfesores
                .Where(hp => hp.Codigo == codigo)
                .OrderByDescending(hp => hp.NumeroSeccion)
                .FirstOrDefault();
            if (horario == null)
                return 0;
            else
                return horario.NumeroSeccion;
        }
    }
}