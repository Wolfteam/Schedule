using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Schedule.API.Filters;
using Schedule.API.Models;
using Schedule.API.Models.Repositories;
using Schedule.Entities;
using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace Schedule.API.Controllers
{
    [Route("api/[controller]")]
    [GlobalAttibute]
    //[AuthenticateAttribute]
    //[AuthorizationAttribute(Entities.Privilegios.Administrador)]
    public class HorarioProfesorController : Controller
    {
        #region Variables
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly string _contentRootPath;
        //TODO: Mover esto al appsettings.json
        private const string _pathModeloPlanificacion = @"\wwwroot\Resources\ModeloPlanificacion.xlsx";
        private const string _savePath = @"\wwwroot\Generated";
        private const string _paExcelFileName = @"\PlanificacionAcademica";
        #endregion
        public HorarioProfesorController(IHostingEnvironment environment)
        {
            _hostingEnvironment = environment;
            _contentRootPath = environment.ContentRootPath;
        }


        [HttpGet("PlanificacionAcademica")]
        public void GeneratePlanificacionAcademica()
        {
            VerifyRecords();
            int limiteSuperior = 0, limiteInferior = 3;
            string periodoAcademico = _unitOfWork.PeriodoCarrera.GetCurrentPeriodo().NombrePeriodo;
            var modelDir = new FileInfo(_contentRootPath + _pathModeloPlanificacion);
            using (var excel = new ExcelPackage(modelDir))
            {
                ExcelWorksheet planificacion = excel.Workbook.Worksheets[0];
                for (int idSemestre = 3; idSemestre <= 14; idSemestre++)
                {
                    string titulo = GetPlanifacionAcademicaReportHeaderTitle(idSemestre, periodoAcademico);
                    planificacion.Cells[String.Format("B{0}:I{0}", limiteInferior - 2)].Value = titulo;
                    var materias = _unitOfWork.Materias.GetBySemestre(idSemestre);
                    var listaHorarios = _unitOfWork.HorarioProfesor.GetBySemestre(idSemestre);
                    foreach (MateriasDTO materia in materias)
                    {
                        var horariosByMateria = listaHorarios.Where(hp => hp.Codigo == materia.Codigo);
                        if (horariosByMateria.Count() == 0)
                            continue;

                        int numeroSecciones = horariosByMateria.Select(h => h.NumeroSeccion).Distinct().Count();
                        var secciones = horariosByMateria.Select(h => h.NumeroSeccion).Distinct();

                        limiteSuperior = limiteInferior + numeroSecciones - 1;
                        string celdaCodigo = String.Format("B{0}:B{1}", limiteInferior, limiteSuperior);
                        string celdaAsignatura = String.Format("C{0}:C{1}", limiteInferior, limiteSuperior);
                        bool merge = true;
                        foreach (int seccion in secciones)
                        {
                            var horarioProfesor = horariosByMateria.Where(h => h.NumeroSeccion == seccion);
                            int horariosDisponibiles = horarioProfesor.Count();
                            if (horariosDisponibiles > 0)
                            {
                                int cantidadAlumnos = horarioProfesor.Select(hp => hp.CantidadAlumnos).FirstOrDefault();
                                string profesor = horarioProfesor.Select(hp => hp.Profesor).FirstOrDefault();
                                string dias = string.Join("\n", horarioProfesor.Select(hp => hp.Dia));
                                string horas = string.Join("\n", horarioProfesor.Select(hp => hp.HoraInicio + " a " + hp.HoraFin));
                                string aulas = string.Join("\n", horarioProfesor.Select(hp => hp.Aula));

                                SetPlanificacionAcademicaReportValues(planificacion, limiteInferior, profesor, seccion, dias,
                                    horas, aulas, cantidadAlumnos, merge, materia.Codigo, materia.Asignatura, celdaCodigo, celdaAsignatura);

                                SetPlanifacionAcademicaReportBodyStyles(planificacion, limiteInferior);
                                merge = false;
                            }
                            limiteInferior++;
                        }
                    }
                    if (idSemestre == 14)
                        break;
                    //Esto copia varias filas y columnas a otra posicion. Cells[RowStart, ColumnStart, RowEnd, ColumnEnd ]
                    planificacion.Cells[1, 2, 2, 9].Copy(planificacion.Cells[limiteInferior + 1, 2, limiteInferior + 2, 9]);
                    limiteInferior += 3;
                }
                //Autofit no funciona con filas merged. Por eso uso WrapText, ademas de que los saltos de linea lo requieren
                planificacion.Cells.AutoFitColumns();
                planificacion.Column(3).Style.WrapText = true;
                for (int i = 6; i <= 8; i++)
                    planificacion.Column(i).Style.WrapText = true;
                SaveExcel(excel, _contentRootPath + _savePath, _paExcelFileName);
            }
        }

        /// <summary>
        /// Verifica si existen registros en la DB, de no ser el caso
        /// los genera
        /// </summary>
        private void VerifyRecords()
        {
            bool recordsExists = _unitOfWork.HorarioProfesor.RecordsExists();
            if (!recordsExists)
                Generate();
        }

        #region Metodos usados para la generacion de los archivos excel
        /// <summary>
        /// Obtiene un titulo acorde al semestre para ser usado en el excel de PlanifacionAcademica
        /// </summary>
        /// <param name="idSemestre">Id del semestre</param>
        /// <param name="periodoAcademico">Periodo academico actual</param>
        /// <returns>Titulo acorde al semestre pasado</returns>
        private string GetPlanifacionAcademicaReportHeaderTitle(int idSemestre, string periodoAcademico)
        {
            string titulo = String.Empty;
            switch (idSemestre)
            {
                case 3:
                    titulo = "Planificación Académica Departamento de Ingeniería de Sistemas " + periodoAcademico + " SEMESTRE III";
                    break;
                case 4:
                    titulo = "Planificación Académica Departamento de Ingeniería de Sistemas " + periodoAcademico + " SEMESTRE IV";
                    break;
                case 5:
                    titulo = "Planificación Académica Departamento de Ingeniería de Sistemas " + periodoAcademico + " SEMESTRE V";
                    break;
                case 6:
                    titulo = "Planificación Académica Departamento de Ingeniería de Sistemas " + periodoAcademico + " SEMESTRE VI";
                    break;
                case 7:
                    titulo = "Planificación Académica Departamento de Ingeniería de Sistemas " + periodoAcademico + " SEMESTRE VII";
                    break;
                case 8:
                    titulo = "Planificación Académica Departamento de Ingeniería de Sistemas " + periodoAcademico + " SEMESTRE VIII";
                    break;
                case 9:
                    titulo = "Planificación Académica Departamento de Ingeniería de Sistemas " + periodoAcademico + " SEMESTRE IX";
                    break;
                case 10:
                    titulo = "Planificación Académica DIS " + periodoAcademico + " Electivas Maquinas Electricas Cod-41";
                    break;
                case 11:
                    titulo = "Planificación Académica DIS " + periodoAcademico + " Electivas Controles Industriales Cod-43";
                    break;
                case 12:
                    titulo = "Planificación Académica DIS " + periodoAcademico + " Electivas Sistemas de Comunicacion Cod-44";
                    break;
                case 13:
                    titulo = "Planificación Académica DIS " + periodoAcademico + " Electivas de Computacion Cod-46";
                    break;
                case 14:
                    titulo = "Planificación Académica DIS " + periodoAcademico + " Asignaturas del DIS a otras carreras";
                    break;
            }
            return titulo;
        }

        /// <summary>
        /// Guarda un excel en particular en la ruta y con 
        /// el nombre que le indiques
        /// </summary>
        /// <param name="excel">Archivo excel a guardar</param>
        /// <param name="rootPath">Ruta raiz de la pagina</param>
        /// <param name="excelFileName">Nombre del archivo (e.g: PlanificacionAcademica)</param>
        private void SaveExcel(ExcelPackage excel, string rootPath, string excelFileName)
        {
            excelFileName = String.Format(excelFileName + "_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd"));
            string outputDir = rootPath + excelFileName;
            FileInfo file = new FileInfo(outputDir);
            if (file.Exists)
                file.Delete();
            // set some document properties
            excel.Workbook.Properties.Title = "Planificacion Academica y Horarios";
            excel.Workbook.Properties.Author = "Efrain Bastidas";
            excel.Workbook.Properties.Comments = "Creado con el fin de aprender..";
            excel.SaveAs(file);
        }

        /// <summary>
        /// Aplica algunos estilos al excel de PlanifacionAcademica
        /// </summary>
        /// <param name="worksheet">Hoja de PlanifacionAcademica</param>
        /// <param name="limiteInferior">Entero usado para calcular el rango de la celda</param>
        public void SetPlanifacionAcademicaReportBodyStyles(ExcelWorksheet worksheet, int limiteInferior)
        {
            string celda = String.Format("B{0}:I{0}", limiteInferior);
            using (ExcelRange range = worksheet.Cells[celda])
            {
                range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
                range.Style.Font.Color.SetColor(Color.White);
                range.Style.Font.Bold = true;
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }
        }

        /// <summary>
        /// Aplica los valores pasados al excel de PlanifacionAcademica
        /// </summary>
        /// <param name="worksheet">Hoja de PlanifacionAcademica</param>
        /// <param name="limiteInferior">Entero usado para calcular el rango de la celda</param>
        /// <param name="nombreProfesor">Nombre del profesor</param>
        /// <param name="seccion">Numero de la seccion</param>
        /// <param name="dias">Dias en los que el profesor da la materia para una seccion</param>
        /// <param name="horas">Horas en las que el profesor da la materia para una seccion<</param>
        /// <param name="aulas">Aulas en las que el profesor da la materia para una seccion<</param>
        /// <param name="cantidadAlumnos">Cantidad de alumnos para una seccion</param>
        /// <param name="merge">Indica si se debe unir las celdas de Codigo y Asignatura</param>
        /// <param name="codigo">Codigo de la materia</param>
        /// <param name="asignatura">Nombre de la materia</param>
        /// <param name="celdaCodigo">Rango de la celda en caso de unirla</param>
        /// <param name="celdaAsignatura">Rango de la celda en caso de unirla</param>
        private void SetPlanificacionAcademicaReportValues(ExcelWorksheet worksheet, int limiteInferior, string nombreProfesor,
            int seccion, string dias, string horas, string aulas, int cantidadAlumnos, bool merge,
            int codigo, string asignatura, string celdaCodigo, string celdaAsignatura)
        {
            if (merge)
            {
                worksheet.Cells[celdaCodigo].Merge = true;
                worksheet.Cells[celdaAsignatura].Merge = true;
                worksheet.Cells[celdaCodigo].Value = codigo;
                worksheet.Cells[celdaAsignatura].Value = asignatura;
            }
            worksheet.Cells["D" + limiteInferior].Value = nombreProfesor;
            worksheet.Cells["E" + limiteInferior].Value = seccion;
            worksheet.Cells["F" + limiteInferior].Value = dias;
            worksheet.Cells["G" + limiteInferior].Value = horas;
            worksheet.Cells["H" + limiteInferior].Value = aulas;
            worksheet.Cells["I" + limiteInferior].Value = cantidadAlumnos;
        }
        #endregion

        #region Metodos usados para la generacion de los horarios y guardado en la DB
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
                                    IdTipoAsignacion = (int)Asignacion.Automatica,
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
                                IdTipoAsignacion = (int)Asignacion.Random,
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
            if (idHoraInicio <= 7 && idHoraFin > 7)
                return false;
            if (idHoraFinDB == idHoraInicio)
                return true;
            int dato1 = idHoraFinDB - idHoraInicio;
            int dato2 = idHoraInicioDB - idHoraFin;
            if ((dato1 >= 0 && dato2 < 0) || (dato1 < 0 && dato2 >= 0))
                return false;
            return true;
        }
        #endregion
    }
}