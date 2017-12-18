using System;
using System.Collections.Generic;
using Schedule.Entities;

namespace Schedule.API.Models.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        AulasRepository AulasRepository { get; }
        CarrerasRepository CarrerasRepository { get; }
        DisponibilidadProfesorRepository DisponibilidadProfesorRepository { get; }
        HorarioProfesorRepository HorarioProfesorRepository { get; }
        MateriasRepository MateriasRepository { get; }
        PeriodoCarreraRepository PeriodoCarreraRepository { get; }
        PrioridadesRepository PrioridadesRepository { get; }
        ProfesorMateriaRepository ProfesorMateriaRepository { get; }
        ProfesorRepository ProfesorRepository { get; }
        SeccionesRepository SeccionesRepository { get; }
        SemestresRepository SemestresRepository { get; }
        UsuarioRepository UsuarioRepository { get; }
        TokenRepository TokenRepository { get; }
        bool Save();
    }
}
