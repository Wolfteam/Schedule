using AutoMapper;
using Schedule.API.Models;
using Schedule.Entities;

namespace Schedule.API.Helpers
{
    /// <summary>
    /// Esta clase se encarga de mapear los modelos de entity framework
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Mapeos comunes
            // Nota como le digo como mapear las propiedades extra que deseo    
            CreateMap<PeriodoCarrera, PeriodoCarreraDTO>();

            CreateMap<PrioridadProfesor, PrioridadProfesorDTO>()
                .ForMember(d => d.CodigoPrioridad, opt => opt.MapFrom(s => s.CodigoPrioridad))
                .ForMember(d => d.HorasACumplir, opt => opt.MapFrom(s => s.HorasACumplir))
                .ForMember(d => d.ID, opt => opt.MapFrom(s => s.IdPrioridad));

            //Nota que le digo que la propiedad Prioridad la mapea de s.PrioridadProfesor, el cual ya fue mapeado arriba
            CreateMap<Profesores, ProfesorDetailsDTO>()
               .ForMember(d => d.Prioridad, opt => opt.MapFrom(s => s.PrioridadProfesor));

            CreateMap<Carreras, CarreraDTO>()
                .ForMember(d => d.IdCarrera, opt => opt.MapFrom(s => s.IdCarrera))
                .ForMember(d => d.NombreCarrera, opt => opt.MapFrom(s => s.NombreCarrera));

            CreateMap<Semestres, SemestreDTO>()
                .ForMember(d => d.IdSemestre, opt => opt.MapFrom(s => s.IdSemestre))
                .ForMember(d => d.NombreSemestre, opt => opt.MapFrom(s => s.NombreSemestre));

            CreateMap<TipoAulaMaterias, TipoAulaMateriaDTO>()
                .ForMember(d => d.IdTipo, opt => opt.MapFrom(s => s.IdTipo))
                .ForMember(d => d.NombreTipo, opt => opt.MapFrom(s => s.NombreTipo));

            CreateMap<Materias, MateriasDetailsDTO>()
               .ForMember(d => d.Carrera, opt => opt.MapFrom(s => s.Carreras))
               .ForMember(d => d.Semestre, opt => opt.MapFrom(s => s.Semestres))
               .ForMember(d => d.TipoMateria, opt => opt.MapFrom(s => s.TipoAulaMaterias));

            CreateMap<Aulas, AulasDetailsDTO>()
                .ForMember(dto => dto.TipoAula, conf => conf.MapFrom(s => s.TipoAulaMateria));

            CreateMap<Aulas, AulasDTO>();

            CreateMap<DisponibilidadProfesores, DisponibilidadProfesorDTO>();

            CreateMap<HorarioProfesores, HorarioProfesorDTO>();

            CreateMap<ProfesoresMaterias, ProfesorMateriaDetailsDTO>()
                .ForMember(dto => dto.Materia, conf => conf.MapFrom(s => s.Materias))
                .ForMember(dto => dto.Profesor, conf => conf.MapFrom(s => s.Profesores));

            CreateMap<Secciones, SeccionesDetailsDTO>()
                .ForMember(dto => dto.Materia, conf => conf.MapFrom(s => s.Materias))
                .ForMember(dto => dto.PeriodoCarrera, conf => conf.MapFrom(s => s.PeriodoCarrera));

            CreateMap<Tokens, TokenDTO>()
                .ForMember(dto => dto.AuthenticationToken, conf => conf.MapFrom(s => s.Token))
                .ForMember(dto => dto.CreateDate, conf => conf.MapFrom(s => s.FechaCreacion))
                .ForMember(dto => dto.ExpiricyDate, conf => conf.MapFrom(s => s.FechaExpiracion))
                .ForMember(dto => dto.Username, conf => conf.MapFrom(s => s.Username));

            #endregion

            #region Mapeos Inversos
            CreateMap<AulasDTO, Aulas>()
                .ForMember(dto => dto.IdTipo, conf => conf.MapFrom(s => s.IdTipo));

            CreateMap<DisponibilidadProfesorDTO, DisponibilidadProfesores>();

            CreateMap<HorarioProfesorDTO, HorarioProfesores>();

            CreateMap<MateriasDTO, Materias>()
               .ForMember(d => d.IdCarrera, opt => opt.MapFrom(s => s.IdCarrera))
               .ForMember(d => d.IdSemestre, opt => opt.MapFrom(s => s.IdSemestre))
               .ForMember(d => d.IdTipo, opt => opt.MapFrom(s => s.IdTipo));

            CreateMap<PeriodoCarreraDTO, PeriodoCarrera>();

            CreateMap<ProfesorDTO, Profesores>()
                .ForMember(d => d.IdPrioridad, opt => opt.MapFrom(s => s.IdPrioridad));

            CreateMap<ProfesorMateriaDTO, ProfesoresMaterias>()
                .ForMember(dto => dto.Id, conf => conf.MapFrom(s => s.Id))
                .ForMember(dto => dto.Cedula, conf => conf.MapFrom(s => s.Cedula))
                .ForMember(dto => dto.Codigo, conf => conf.MapFrom(s => s.Codigo));

            CreateMap<SeccionesDTO, Secciones>();
            #endregion

        }
    }
}
