using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Schedule.API.Filters;
using Schedule.API.Models;
using Schedule.API.Models.Repositories;
using Schedule.Entities;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Schedule.API.Controllers
{
    [Route("api/[controller]")]
    [GlobalAttibute]
    //[AuthenticateAttribute]
    //[AuthorizationAttribute(Entities.Privilegios.Administrador)]
    public class HorarioProfesorController : Controller
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        [HttpGet("Generate")]
        public IActionResult Generate(bool fromStart)
        {
            bool recordsExists = _unitOfWork.HorarioProfesor.RecordsExists();
            if (!recordsExists)
            {
                Generate();
            }
            return Ok();
        }

        //TODO: Aveces no funciona bien lo de que la materia no se encuentre en el rango del medidoia

        /// <summary>
        /// Genera los horarios de los profesores acorde a su prioridad, semestre
        /// y materia. Por los momentos si no se puede generar un horario para un profesor
        /// y el mismo tiene prioridad < 5 no se le genera nada.
        /// </summary>
        private void Generate()
        {
            int idPeriodo = _unitOfWork.PeriodoCarrera.GetCurrentPeriodo().IdPeriodo;
            var secciones = _unitOfWork.Secciones.Get();

            if (secciones.Count() == 0)
                return;

            for (byte idPrioridad = 1; idPrioridad <= 6; idPrioridad++)
            {
                for (byte idSemestre = 3; idSemestre <= 14; idSemestre++)
                {
                    var seccionesBySemestre = secciones.Where(sec => sec.Materia.Semestre.IdSemestre == idSemestre);
                    foreach (var seccion in seccionesBySemestre)
                    {
                        var disponibilidades = _unitOfWork.DisponibilidadProfesor.GetByPrioridadMateria(idPrioridad, seccion.Materia.Codigo);
                        var cedulas = disponibilidades.Select(ci => ci.Cedula).Distinct();
                        int numeroSeccion = 1;
                        for (int i = 1; i <= seccion.NumeroSecciones; i++)
                        {
                            foreach (uint cedula in cedulas)
                            {
                                if (numeroSeccion > seccion.NumeroSecciones)
                                    break;

                                var disp = disponibilidades.Where(d => d.Cedula == cedula);
                                int horasAsignadas = _unitOfWork.HorarioProfesor.CalculateHorasAsignadas(cedula);
                                byte horasRestantes = (byte)(disp.FirstOrDefault().HorasACumplir - horasAsignadas);

                                if (horasRestantes < seccion.Materia.HorasAcademicasSemanales)
                                    break;

                                var aulas = _unitOfWork.Aulas.GetByTipoCapacidad(seccion.Materia.TipoMateria.IdTipo, seccion.CantidadAlumnos);
                                if (aulas.Count() == 0)
                                    break;

                                bool result = false;
                                result = GenerateHorario(cedula, seccion.Materia.Codigo, idSemestre,
                                    seccion.Materia.TipoMateria.IdTipo, seccion.Materia.HorasAcademicasSemanales,
                                    (byte)numeroSeccion, disp.FirstOrDefault().Disponibilidad, aulas, idPeriodo);

                                if (!result)
                                {
                                    //Creo que solo los DE y los 5 se les puede asignar un horario random
                                    if (idPrioridad < 5)
                                    {
                                        break;
                                    }
                                    //debo guardar los datos del prof q no pude asignar
                                    result = GenerateRandomHorario(cedula, seccion.Materia.Codigo, idSemestre,
                                        seccion.Materia.TipoMateria.IdTipo, seccion.Materia.HorasAcademicasSemanales,
                                        (byte)numeroSeccion, aulas, idPeriodo);
                                    if (!result)
                                    {
                                        break;
                                    }
                                }
                                if (result)
                                    numeroSeccion++;
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Calcula cuantas horas debe tener una materia diariamente en base
        /// a los dias disponibles del profesor y el tipo de materia
        /// </summary>
        /// <param name="horasPorSemana">Horas por semana de la materia</param>
        /// <param name="diasDisponibles">Numero de dias disponibles en la semana</param>
        /// <param name="idTipoMateria">Tipo materia</param>
        /// <returns>Horas por semana de una materia en una seccion</returns>
        private int CalculateHorasDiarias(int horasPorSemana, int diasDisponibles, int idTipoMateria)
        {
            if (diasDisponibles >= 2 && idTipoMateria == 1)
                return (int)Math.Round((horasPorSemana / 2.0));
            else
                return horasPorSemana;
        }

        /// <summary>
        /// Genera el horario de una seccion a un profesor de una materia, teniendo en
        /// cuenta su disponibilidad, para ello se pasa por 3 validaciones principales.
        /// </summary>
        /// <param name="cedula">Cedula del profesor</param>
        /// <param name="codigo">Codigo de la materia</param>
        /// <param name="idSemestre">Id del semestre</param>
        /// <param name="idTipoMateria">Id del tipo de materia</param>
        /// <param name="horasPorSemana">Numero de horas por semana que debe tener la materia</param>
        /// <param name="seccion">Numero de la seccion de la materia</param>
        /// <param name="disponibilidad">Disponibilidad del profesor</param>
        /// <param name="aulas">Aulas acorde a la materia</param>
        /// <param name="idPeriodo">Id del periodo academico actual</param>
        /// <returns>True en caso de haberse generado y guardado los horarios generados</returns>
        private bool GenerateHorario(uint cedula, ushort codigo, byte idSemestre, byte idTipoMateria,
            byte horasPorSemana, byte seccion, IEnumerable<DisponibilidadProfesorDTO> disponibilidad,
            IEnumerable<AulasDTO> aulas, int idPeriodo)
        {
            byte idHoraInicio = 0;
            byte idHoraFin = 0;
            byte aux = horasPorSemana;
            int diasDisponibles = disponibilidad.Select(d => d.IdDia).Distinct().Count();
            int horasPorSeccion = CalculateHorasDiarias(horasPorSemana, diasDisponibles, idTipoMateria);
            int numeroRegistros = disponibilidad.Count();
            bool horaAsignadaPorDia = false;
            List<bool> results;
            List<HorarioProfesorDTO> horariosGenerados = new List<HorarioProfesorDTO>();
            IEnumerable<byte> dias = disponibilidad.Select(d => d.IdDia).Distinct();

            foreach (byte dia in dias)
            {
                foreach (var disp in disponibilidad)
                {
                    if (disp.IdDia != dia)
                        continue;
                    idHoraInicio = disp.IdHoraInicio;
                    idHoraFin = (byte)(idHoraInicio + horasPorSeccion);
                    while (idHoraFin <= disp.IdHoraFin)
                    {
                        foreach (var aula in aulas)
                        {
                            results = new List<bool>();
                            results.Add(ValidateChoqueAula(idHoraInicio, idHoraFin, dia, aula.IdAula));
                            if (results.Contains(false))
                                continue;
                            results.Add(ValidateChoqueSemestre(codigo, idSemestre, idHoraInicio, idHoraFin, dia));
                            results.Add(ValidateChoqueHorario(cedula, idHoraInicio, idHoraFin, dia));
                            if (results.Contains(false))
                                break;

                            horariosGenerados.Add(
                                new HorarioProfesorDTO
                                {
                                    Cedula = cedula,
                                    Codigo = codigo,
                                    IdAula = aula.IdAula,
                                    IdDia = dia,
                                    IdHoraFin = idHoraFin,
                                    IdHoraInicio = idHoraInicio,
                                    IdPeriodo = idPeriodo,
                                    NumeroSeccion = seccion
                                });

                            aux = (byte)(aux - horasPorSeccion);
                            horasPorSeccion = aux;
                            horaAsignadaPorDia = true;
                            break;
                        }
                        if (horaAsignadaPorDia || aux == 0)
                        {
                            horaAsignadaPorDia = false;
                            break;
                        }
                        idHoraInicio++;
                        idHoraFin = (byte)(idHoraInicio + horasPorSeccion);
                    }
                    if ((aux != horasPorSemana && aux > 0) || (aux == 0))
                        break;
                }
                if (aux == 0)
                    break;
            }
            if (aux != 0)
                return false;

            return _unitOfWork.HorarioProfesor
                .Create(Mapper.Map<IEnumerable<HorarioProfesorDTO>, IEnumerable<HorarioProfesores>>(horariosGenerados));
        }

        /// <summary>
        /// Genera el horario de una seccion a un profesor de una materia
        /// de forma aleatoria, para ello se pasa por 3 validaciones principales.
        /// </summary>
        /// <param name="cedula">Cedula del profesor</param>
        /// <param name="codigo">Codigo de la materia</param>
        /// <param name="idSemestre">Id del semestre</param>
        /// <param name="idTipoMateria">Id del tipo de materia</param>
        /// <param name="horasPorSemana">Numero de horas por semana que debe tener la materia</param>
        /// <param name="seccion">Numero de la seccion de la materia</param>
        /// <param name="aulas">Aulas acorde a la materia</param>
        /// <param name="idPeriodo">Id del periodo academico actual</param>       
        /// <returns>True en caso de haberse generado y guardado los horarios generados</returns>
        private bool GenerateRandomHorario(uint cedula, ushort codigo, byte idSemestre, byte idTipoMateria,
            byte horasPorSemana, byte seccion, IEnumerable<AulasDTO> aulas, int idPeriodo)
        {
            byte idHoraInicio = 0;
            byte idHoraFin = 0;
            byte aux = horasPorSemana;
            int horasPorSeccion = CalculateHorasDiarias(horasPorSemana, 2, idTipoMateria);
            bool horaAsignadaPorDia = false;
            List<bool> results;
            List<HorarioProfesorDTO> horariosGenerados = new List<HorarioProfesorDTO>();

            for (byte idDia = 1; idDia < 7; idDia++)
            {
                idHoraInicio = 1;
                idHoraFin = (byte)(idHoraInicio + horasPorSeccion);
                while (idHoraFin <= 14)
                {
                    foreach (var aula in aulas)
                    {
                        results = new List<bool>();
                        results.Add(ValidateChoqueAula(idHoraInicio, idHoraFin, idDia, aula.IdAula));
                        if (results.Contains(false))
                            continue;

                        results.Add(ValidateChoqueSemestre(codigo, idSemestre, idHoraInicio, idHoraFin, idDia));
                        results.Add(ValidateChoqueHorario(cedula, idHoraInicio, idHoraFin, idDia));
                        if (results.Contains(false))
                            break;

                        horariosGenerados.Add(
                            new HorarioProfesorDTO
                            {
                                Cedula = cedula,
                                Codigo = codigo,
                                IdAula = aula.IdAula,
                                IdDia = idDia,
                                IdHoraFin = idHoraFin,
                                IdHoraInicio = idHoraInicio,
                                IdPeriodo = idPeriodo,
                                NumeroSeccion = seccion
                            });

                        aux = (byte)(aux - horasPorSeccion);
                        horasPorSeccion = aux;
                        horaAsignadaPorDia = true;
                        break;
                    }
                    if (horaAsignadaPorDia || aux == 0)
                    {
                        horaAsignadaPorDia = false;
                        break;
                    }
                    idHoraInicio++;
                    idHoraFin = (byte)(idHoraInicio + horasPorSeccion);
                }
                if (aux == 0)
                    break;
            }
            if (aux != 0)
                return false;

            return _unitOfWork.HorarioProfesor
                .Create(Mapper.Map<IEnumerable<HorarioProfesorDTO>, IEnumerable<HorarioProfesores>>(horariosGenerados));
        }

        /// <summary>
        /// Valida de que no este ocupada una cierta aula 
        /// a una cierta hora, en un cierto dia.
        /// </summary>
        /// <param name="idHoraInicio">Id de la hora de inicio</param>
        /// <param name="idHoraFin">Id de la hora de fin</param>
        /// <param name="idDia">Id del dia</param>
        /// <param name="idAula">Id del aula</param>
        /// <returns>True en caso de no estar el aula ocupada</returns>
        private bool ValidateChoqueAula(byte idHoraInicio, byte idHoraFin, byte idDia, byte idAula)
        {
            var horarioProfesores = _unitOfWork.HorarioProfesor.GetByDiaAula(idDia, idAula);
            bool result = true;
            foreach (var horario in horarioProfesores)
            {
                result = ValidateHoras(horario.IdHoraInicio, horario.IdHoraFin, idHoraInicio, idHoraFin);
                if (!result)
                    return false;
            }
            return result;
        }

        /// <summary>
        /// Valida que un profesor no pueda dar varias clases al mismo tiempo y en el mismo dia.
        /// </summary>
        /// <param name="cedula">Cedula del profesor</param>
        /// <param name="idHoraInicio">Id de la hora de inicio</param>
        /// <param name="idHoraFin">Id de la hora de fin</param>
        /// <param name="idDia">Id del dia</param>
        /// <returns>True en caso de no existir choques de horario</returns>
        private bool ValidateChoqueHorario(uint cedula, byte idHoraInicio, byte idHoraFin, byte idDia)
        {
            var horarios = _unitOfWork.HorarioProfesor.GetByCedulaDia(cedula, idDia);
            bool result = true;
            if (horarios.FirstOrDefault(h => h.IdHoraInicio == idHoraInicio && h.IdHoraFin == idHoraFin) != null)
                return !result;
            foreach (var horario in horarios)
            {
                result = ValidateHoras(horario.IdHoraInicio, horario.IdHoraFin, idHoraInicio, idHoraFin);
                if (!result)
                    return result;
            }
            return result;
        }

        /// <summary>
        /// Valida que no existan materias del mismo semestre en el mismo dia
        /// y a la misma hora que coincidan con las que se pasan en los parametros.
        /// Si una materia es electiva, devolvera siempre true.
        /// Una duda es si deberia ser valido que una materia tenga por ejemplo 2 secciones
        /// a la misma hora,de la forma como esta actualmente, si ocurre dicho caso devuelve false
        /// </summary>
        /// <param name="codigo">Codigo de la materia</param>
        /// <param name="idSemestre">Id del semestre</param>
        /// <param name="idHoraInicio">Id de la hora de inicio</param>
        /// <param name="idHoraFin">Id de la hora de fin</param>
        /// <param name="idDia">Id del dia</param>
        /// <returns>True en caso de no existir choque de semestre</returns>
        public bool ValidateChoqueSemestre(ushort codigo, byte idSemestre, byte idHoraInicio, byte idHoraFin, byte idDia)
        {
            bool result = true;
            if (idSemestre > 9)
                return result;
            var horarios = _unitOfWork.HorarioProfesor.GetBySemestreDia(idSemestre, idDia);
            foreach (var horario in horarios)
            {
                result = ValidateHoras(horario.IdHoraInicio, horario.IdHoraFin, idHoraInicio, idHoraFin);
                if (!result)
                    return result;
            }
            return result;
        }

        /// <summary>
        /// Valida las horas que recibe como parametros.
        /// Una hora no es valida cuando alguno de los datos da negativo y la otra da positiva.
        /// Si alguna da cero, es valida.
        /// Si ambas dan negativa es valida.
        /// Agregue otra validacion para casos cuando una materia comienza seguida de otra. Debe ser testeado
        /// Tambien verifica que las horas de inicio y fin no se encuentren en el rango del mediodia
        /// </summary>
        /// <param name="idHoraInicioDB">Id de la hora de inicio almacenada en la DB</param>
        /// <param name="idHoraFinDB">Id de la hora de fin almacenada en la DB</param>
        /// <param name="idHoraInicio">Id de la hora de inicio</param>
        /// <param name="idHoraFin">Id de la hora de fin</param>
        /// <returns>True si las horas dadas son validas.</returns>
        private bool ValidateHoras(byte idHoraInicioDB, byte idHoraFinDB, byte idHoraInicio, byte idHoraFin)
        {
            if (idHoraFinDB == idHoraInicio)
                return true;
            if (idHoraInicio <= 7 && idHoraFin > 7)
                return false;
            int dato1 = idHoraFinDB - idHoraInicio;
            int dato2 = idHoraInicioDB - idHoraFin;
            if ((dato1 >= 0 && dato2 < 0) || (dato1 < 0 && dato2 >= 0))
                return false;
            return true;
        }
    }
}