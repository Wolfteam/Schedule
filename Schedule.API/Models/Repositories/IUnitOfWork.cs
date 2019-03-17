using System;
using System.Collections.Generic;
using Schedule.Entities;

namespace Schedule.API.Models.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IAulasRepository AulasRepository { get; }
        ICarrerasRepository CarrerasRepository { get; }
        IDisponibilidadProfesorRepository DisponibilidadProfesorRepository { get; }
        IHorarioProfesorRepository HorarioProfesorRepository { get; }
        IMateriasRepository MateriasRepository { get; }
        IPeriodoCarreraRepository PeriodoCarreraRepository { get; }
        IPrioridadesRepository PrioridadesRepository { get; }
        IPrivilegiosRepository PrivilegiosRepository { get; }
        IProfesorMateriaRepository ProfesorMateriaRepository { get; }
        IProfesorRepository ProfesorRepository { get; }
        ISeccionesRepository SeccionesRepository { get; }
        ISemestresRepository SemestresRepository { get; }
        IUsuarioRepository UsuarioRepository { get; }
        ITipoAulaMateriaRepository TipoAulaMateriaRepository { get; }
        ITokenRepository TokenRepository { get; }
        bool Save();
    }
}
