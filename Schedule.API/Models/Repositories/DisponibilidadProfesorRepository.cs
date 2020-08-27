using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Schedule.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Schedule.API.Models.Repositories
{
    public class DisponibilidadProfesorRepository
        : Repository<DisponibilidadProfesores>, IDisponibilidadProfesorRepository
    {
        private readonly IMapper _mapper;

        public HorariosContext HorariosContext
        {
            get { return _context as HorariosContext; }
        }

        public DisponibilidadProfesorRepository(HorariosContext context, IMapper mapper)
            : base(context)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Crea una nueva disponibilidad a un profesor en particular
        /// </summary>
        /// <param name="disponibilidad">Objeto de tipo DisponibilidadProfesores</param>
        public override void AddRange(IEnumerable<DisponibilidadProfesores> disponibilidad)
        {
            RemoveByCedula(disponibilidad.FirstOrDefault().Cedula);
            base.AddRange(disponibilidad);
        }

        /// <summary>
        /// Obtiene una lista de todas las disponibilidades de los profesores
        /// </summary>
        /// <returns>IEnumerable de DisponibilidadProfesorDTO</returns>
        public IEnumerable<DisponibilidadProfesorDetailsDTO> GetAllCurrent()
        {
            var disponibilidades = HorariosContext.DisponibilidadProfesores
                 .Include(pc => pc.PeriodoCarrera)
                 .Include(p => p.Profesores.PrioridadProfesor)
                 .Where(c => c.PeriodoCarrera.Status == true);

            var cedulas = disponibilidades.Select(d => d.Cedula).Distinct().ToList();

            var result = new List<DisponibilidadProfesorDetailsDTO>();
            foreach (uint cedula in cedulas)
            {
                var disp = disponibilidades.Where(d => d.Cedula == cedula);
                var dto = new DisponibilidadProfesorDetailsDTO
                {
                    Cedula = cedula,
                    Disponibilidad = disp.ProjectTo<DisponibilidadProfesorDTO>(_mapper.ConfigurationProvider).ToList(),
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
        /// <returns>DisponibilidadProfesorDetailsDTO</returns>
        public DisponibilidadProfesorDetailsDTO GetByCedula(int cedula)
        {
            var disponibilidad = HorariosContext.DisponibilidadProfesores
                .Where(c => c.Cedula == cedula && c.PeriodoCarrera.Status)
                .ProjectTo<DisponibilidadProfesorDTO>(_mapper.ConfigurationProvider)
                .ToList();

            var result = new DisponibilidadProfesorDetailsDTO
            {
                Cedula = (uint)cedula,
                Disponibilidad = disponibilidad,
                HorasAsignadas =
                    (byte)(disponibilidad.Sum(hf => hf.IdHoraFin) - disponibilidad.Sum(hi => hi.IdHoraInicio))
            };

            return result;
        }

        /// <summary>
        /// Obtiene una lista con todas las disponibilidad del profesor indicado en el dia indicado
        /// </summary>
        /// <param name="cedula">Cedula del profesor</param>
        /// <param name="idDia">Id del dia a buscar</param>
        /// <returns>DisponibilidadProfesorDetailsDTO</returns>
        public DisponibilidadProfesorDetailsDTO GetByCedulaDia(int cedula, byte idDia)
        {
            var disponibilidad = HorariosContext.DisponibilidadProfesores
                .Include(p => p.Profesores.PrioridadProfesor)
                .Where(c => c.Cedula == cedula && c.PeriodoCarrera.Status == true && c.IdDia == idDia)
                .ToList();

            var result = new DisponibilidadProfesorDetailsDTO
            {
                Cedula = (uint)cedula
            };
            if (!disponibilidad.Any())
                return result;

            result.Disponibilidad = _mapper.Map<List<DisponibilidadProfesorDTO>>(disponibilidad);
            result.HorasACumplir = disponibilidad.FirstOrDefault().Profesores.PrioridadProfesor.HorasACumplir;
            result.HorasAsignadas = (byte)(disponibilidad.Sum(hf => hf.IdHoraFin) - disponibilidad.Sum(hi => hi.IdHoraInicio));

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
            var disponibilidades = HorariosContext.DisponibilidadProfesores
                .Include(pc => pc.PeriodoCarrera)
                .Include(pm => pm.Profesores.ProfesoresMaterias)
                .Include(p => p.Profesores.PrioridadProfesor)
                .Where
                (
                    c => c.PeriodoCarrera.Status == true &&
                    c.Profesores.PrioridadProfesor.IdPrioridad == idPrioridad &&
                    c.Profesores.ProfesoresMaterias.FirstOrDefault(pm => pm.Codigo == codigo) != null
                );

            var cedulas = disponibilidades.Select(d => d.Cedula).Distinct().ToList();

            var result = new List<DisponibilidadProfesorDetailsDTO>();
            foreach (uint cedula in cedulas)
            {
                var disp = disponibilidades.Where(d => d.Cedula == cedula);
                var dto = new DisponibilidadProfesorDetailsDTO
                {
                    Cedula = cedula,
                    Disponibilidad = disp.ProjectTo<DisponibilidadProfesorDTO>(_mapper.ConfigurationProvider).ToList(),
                    HorasACumplir = disp.FirstOrDefault(d => d.Cedula == cedula).Profesores.PrioridadProfesor.HorasACumplir,
                    HorasAsignadas = (byte)(disp.Sum(hf => hf.IdHoraFin) - disp.Sum(hi => hi.IdHoraInicio))
                };
                result.Add(dto);
            }
            return result;
        }

        /// <summary>
        /// Obtiene las horas asignadas para un profesor en particular
        /// </summary>
        /// <param name="cedula">Cedula del profesor</param>
        /// <returns></returns>
        public byte GetHorasAsignadas(int cedula)
        {
            var query = HorariosContext.DisponibilidadProfesores
                    .Where(c => c.Cedula == cedula && c.PeriodoCarrera.Status == true)
                    .Select(d => new { d.IdHoraInicio, d.IdHoraFin });
            return (byte)(query.Sum(hf => hf.IdHoraFin) - query.Sum(hi => hi.IdHoraInicio));
        }

        /// <summary>
        /// Borra todas las disponibilidades de un profesor
        /// </summary>
        /// <param name="cedula">Cedula del profesor</param>
        public void RemoveByCedula(uint cedula)
        {
            HorariosContext.DisponibilidadProfesores
            .RemoveRange
            (
                HorariosContext.DisponibilidadProfesores
                .Include(pc => pc.PeriodoCarrera)
                .Where(x => x.Cedula == cedula && x.PeriodoCarrera.Status == true)
            );
            HorariosContext.SaveChanges();
        }
    }
}