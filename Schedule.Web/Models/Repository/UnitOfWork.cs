using System;
using System.Net.Http;
using Schedule.Web.Models.Repository;

namespace Schedule.Web.Models.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Constantes
        private const string _apiAulas = "api/Aulas";
        private const string _apiCarreras = "api/Carreras";
        private const string _apiDisponibilidad = "api/Disponibilidad";
        private const string _apiHorarioProfesor = "api/HorarioProfesor";
        private const string _apiMaterias = "api/Materias";
        private const string _apiPeriodoCarrera = "api/PeriodoCarrera";
        private const string _apiPrioridades = "api/Prioridades";
        private const string _apiProfesorMateria = "api/ProfesorMateria";
        private const string _apiProfesor = "api/Profesor";
        private const string _apiSecciones = "api/Secciones";
        private const string _apiSemestres = "api/Semestres";
        #endregion

        #region  Propiedades
        public IAulasRepository AulasRepository { get; private set; }
        public ICarrerasRepository CarrerasRepository { get; private set; }
        public IDisponibilidadProfesorRepository DisponibilidadRepository { get; private set; }
        public IHorarioProfesorRepository HorarioProfesorRepository { get; private set; }
        public IMateriasRepository MateriasRepository { get; private set; }
        public IPeriodoCarreraRepository PeriodoCarreraRepository { get; private set; }
        public IPrioridadesRepository PrioridadesRepository { get; private set; }
        public IProfesorMateriaRepository ProfesorMateriaRepository { get; private set; }
        public IProfesorRepository ProfesorRepository { get; private set; }
        public ISeccionesRepository SeccionesRepository { get; private set; }
        public ISemestresRepository SemestresRepository { get; private set; }
        #endregion

        public UnitOfWork(HttpClient httpClient)
        {
            AulasRepository = new AulasRepository(httpClient, _apiAulas);
            CarrerasRepository = new CarrerasRepository(httpClient, _apiCarreras);
            DisponibilidadRepository = new DisponibilidadProfesorRepository(httpClient, _apiDisponibilidad);
            HorarioProfesorRepository = new HorarioProfesorRepository(httpClient, _apiHorarioProfesor);
            MateriasRepository = new MateriasRepository(httpClient, _apiMaterias);
            PeriodoCarreraRepository = new PeriodoCarreraRepository(httpClient, _apiPeriodoCarrera);
            PrioridadesRepository = new PrioridadesRepository(httpClient, _apiPrioridades);
            ProfesorMateriaRepository = new ProfesorMateriaRepository(httpClient, _apiProfesorMateria);
            ProfesorRepository = new ProfesorRepository(httpClient, _apiProfesor);
            SeccionesRepository = new SeccionesRepository(httpClient, _apiSecciones);
            SemestresRepository = new SemestresRepository(httpClient, _apiSemestres);
        }
    }
}