using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Schedule.API.Models;
using Schedule.Entities;
using System.Collections.Generic;
using System;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Schedule.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = Roles.ADMINISTRADOR + ", " + Roles.PROFESOR)]
    public class HorarioProfesorController : BaseController
    {
        #region Variables
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly string _contentRootPath;
        private ExcelHorarioProfesorSettings _excelSettings;
        private const string _tituloPA = "PLANIFICACION ACADEMICA DEPARTAMENTO DE INGENIERIA DE SISTEMAS PERIODO ";
        private const string _tituloPH = "HORARIO DEPARTAMENTO DE INGENIERIA DE SISTEMAS PERIODO ";
        #endregion
        public HorarioProfesorController(IHostingEnvironment environment, 
            IOptions<AppSettings> appSettings, HorariosContext context)
            :base (context)
        {
            _hostingEnvironment = environment;
            _contentRootPath = environment.ContentRootPath;
            _excelSettings = appSettings.Value.ExcelHorarioProfesorSettings;
        }

        [HttpGet("PlanificacionAcademica")]
        public IActionResult GeneratePlanificacionAcademica()
        {
            VerifyRecords();
            int limiteSuperior = 0, limiteInferior = 3;
            string periodoAcademico = _db.PeriodoCarreraRepository.GetCurrentPeriodo().NombrePeriodo;
            var modelDir = new FileInfo(_contentRootPath + _excelSettings.ModeloPlanificacionPath);
            string filePath;
            using (var excel = new ExcelPackage(modelDir))
            {
                ExcelWorksheet planificacion = excel.Workbook.Worksheets[0];
                excel.Workbook.Worksheets.Delete("PlanificacionAulas");
                excel.Workbook.Worksheets.Delete("PlanificacionHorarios");
                for (int idSemestre = 3; idSemestre <= 14; idSemestre++)
                {
                    string titulo = GetPlanifacionAcademicaReportHeaderTitle(idSemestre, _tituloPA, periodoAcademico);
                    planificacion.Cells[String.Format("B{0}:I{0}", limiteInferior - 2)].Value = titulo;
                    var materias = _db.MateriasRepository.GetBySemestre(idSemestre);
                    var listaHorarios = _db.HorarioProfesorRepository.GetBySemestre(idSemestre);
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
                //planificacion.Cells.AutoFitColumns();
                planificacion.Column(3).Style.WrapText = true;
                for (int i = 6; i <= 8; i++)
                    planificacion.Column(i).Style.WrapText = true;
                filePath = SaveExcel(excel, _contentRootPath + _excelSettings.SavePath, _excelSettings.PlanificacionAcademicaExcelFileName);
            }
            return GetExcel(filePath, _excelSettings.PlanificacionAcademicaExcelFileName + ".xlsx");
        }

        [HttpGet("PlanificacionAulas")]
        public IActionResult GeneratePlanificacionAulas()
        {
            VerifyRecords();
            int contador = 0, puntero = 3, index = 0;
            string periodoAcademico = _db.PeriodoCarreraRepository.GetCurrentPeriodo().NombrePeriodo;
            var modelDir = new FileInfo(_contentRootPath + _excelSettings.ModeloPlanificacionPath);
            string filePath;
            using (var excel = new ExcelPackage(modelDir))
            {
                ExcelWorksheet planificacion = excel.Workbook.Worksheets["PlanificacionAulas"];
                excel.Workbook.Worksheets.Delete("PlanificacionAcademica");
                excel.Workbook.Worksheets.Delete("PlanificacionHorarios");
                var aulas = _db.AulasRepository.GetAll().OrderBy(a => a.NombreAula);
                foreach (var aula in aulas)
                {
                    string titulo = $"PLANIFICACION AULA {aula.NombreAula} DPTO. DE  INGENIERIA DE SISTEMA PERIODO {periodoAcademico}";
                    planificacion.Cells[$"B{contador + 1}:I{contador + 1}"].Value = titulo;
                    var horarios = _db.HorarioProfesorRepository.GetByAula(aula.IdAula);
                    foreach (HorarioProfesorDetailsDTO horario in horarios)
                    {
                        for (int idDia = 1; idDia < 7; idDia++)
                        {
                            for (int idHora = 1; idHora < 14; idHora++)
                            {
                                if (horario.IdDia == idDia && horario.IdHoraInicio == idHora)
                                {
                                    string value = $"{horario.Asignatura} \n {horario.Codigo} \n S-{horario.NumeroSeccion}";
                                    int rowStart = puntero + contador;
                                    int rowEnd = rowStart + horario.IdHoraFin - horario.IdHoraInicio - 1;
                                    planificacion.Cells[rowStart, idDia + 3, rowEnd, idDia + 3].Merge = true;
                                    planificacion.Cells[rowStart, idDia + 3, rowEnd, idDia + 3].Value = value;
                                    puntero = rowEnd + 1;
                                    break;
                                }
                                else
                                    puntero++;
                            }
                            puntero = 3;
                        }
                    }
                    //Como comparten los mismos estilos se le aplican.
                    SetPlanifacionHorarioReportBodyStyles(planificacion, contador + 1, contador + 15);
                    //Si ya inserte todas las aulas me salgo
                    if (index == aulas.Count() - 1)
                        break;
                    index++;
                    contador += 16;
                    puntero = 3;
                    //Copia la cabecera
                    planificacion.Cells[1, 2, 2, 9].Copy(planificacion.Cells[puntero + contador - 2, 2, puntero + contador - 1, 9]);

                    //Copia la fila del almuerzo
                    planificacion.Cells[9, 4, 9, 9].Copy(planificacion.Cells[contador + 9, 4, contador + 9, 9]);

                    //Copia la columna B de las horas y realizo los merge
                    planificacion.Cells[3, 2, 15, 2].Copy(planificacion.Cells[puntero + contador, 2, puntero + contador + 12, 2]);
                    for (int i = puntero + contador; i <= puntero + contador + 12; i++)
                    {
                        if (i == puntero + contador + 6)
                            planificacion.Cells[$"D{i}:I{i}"].Merge = true;
                        planificacion.Cells[$"B{i}:C{i}"].Merge = true;
                    }
                }
                filePath = SaveExcel(excel, _contentRootPath + _excelSettings.SavePath, _excelSettings.PlanificacionAulasExcelFileName);
            }
            return GetExcel(filePath, _excelSettings.PlanificacionAulasExcelFileName + ".xlsx");
        }

        [HttpGet("PlanificacionHorario")]
        public IActionResult GeneratePlanificacionHorario()
        {
            VerifyRecords();
            int contador = 0, puntero = 3;
            string periodoAcademico = _db.PeriodoCarreraRepository.GetCurrentPeriodo().NombrePeriodo;
            var modelDir = new FileInfo(_contentRootPath + _excelSettings.ModeloPlanificacionPath);
            string filePath;
            using (var excel = new ExcelPackage(modelDir))
            {
                ExcelWorksheet planificacion = excel.Workbook.Worksheets[1];
                excel.Workbook.Worksheets.Delete("PlanificacionAcademica");
                excel.Workbook.Worksheets.Delete("PlanificacionAulas");
                for (int idSemestre = 3; idSemestre <= 14; idSemestre++)
                {
                    string titulo = GetPlanifacionAcademicaReportHeaderTitle(idSemestre, _tituloPH, periodoAcademico);
                    planificacion.Cells[String.Format("B{0}:I{0}", contador + 1)].Value = titulo;
                    var horarios = _db.HorarioProfesorRepository.GetBySemestre(idSemestre);
                    foreach (var horario in horarios)
                    {
                        for (int idDia = 1; idDia < 7; idDia++)
                        {
                            for (int idHora = 1; idHora < 14; idHora++)
                            {
                                if (horario.IdDia == idDia && horario.IdHoraInicio == idHora)
                                {
                                    string value = String.Format("{0} \n {1} \n S-{2}", horario.Asignatura, horario.Codigo, horario.NumeroSeccion);
                                    int rowStart = puntero + contador;
                                    int rowEnd = rowStart + horario.IdHoraFin - horario.IdHoraInicio - 1;
                                    planificacion.Cells[rowStart, idDia + 3, rowEnd, idDia + 3].Merge = true;
                                    planificacion.Cells[rowStart, idDia + 3, rowEnd, idDia + 3].Value = value;
                                    puntero = rowEnd + 1;
                                    break;
                                }
                                else
                                    puntero++;
                            }
                            puntero = 3;
                        }
                    }
                    SetPlanifacionHorarioReportBodyStyles(planificacion, contador + 1, contador + 15);
                    contador += 16;
                    puntero = 3;
                    if (idSemestre == 14)
                        break;

                    //Copia la cabecera
                    planificacion.Cells[1, 2, 2, 9].Copy(planificacion.Cells[puntero + contador - 2, 2, puntero + contador - 1, 9]);

                    //Copia la fila del almuerzo
                    planificacion.Cells[9, 4, 9, 9].Copy(planificacion.Cells[contador + 9, 4, contador + 9, 9]);

                    //Copia la columna B de las horas y realizo los merge
                    planificacion.Cells[3, 2, 15, 2].Copy(planificacion.Cells[puntero + contador, 2, puntero + contador + 12, 2]);
                    for (int i = puntero + contador; i <= puntero + contador + 12; i++)
                    {
                        if (i == puntero + contador + 6)
                            planificacion.Cells[String.Format("D{0}:I{0}", i)].Merge = true;
                        planificacion.Cells[String.Format("B{0}:C{0}", i)].Merge = true;
                    }
                }
                filePath = SaveExcel(excel, _contentRootPath + _excelSettings.SavePath, _excelSettings.PlanificacionHorariosExcelFileName);
            }
            return GetExcel(filePath, _excelSettings.PlanificacionHorariosExcelFileName + ".xlsx");
        }

        /// <summary>
        /// Obtiene el archivo excel especifado en la ruta
        /// </summary>
        /// <param name="filePath">Ruta completa del archivo excel</param>
        /// <param name="returnFileName">Nombre con el cual se devolvera el archivo al cliente</param>
        /// <returns>FileContentResult con el nombre especificado de retorno</returns>
        private FileContentResult GetExcel(string filePath, string returnFileName)
        {
            var mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, mimeType, returnFileName);
        }

        /// <summary>
        /// Verifica si existen registros en la DB, de no ser el caso
        /// los genera
        /// </summary>
        private void VerifyRecords()
        {
            bool recordsExists = _db.HorarioProfesorRepository.RecordsExists();
            if (!recordsExists)
                Generate();
        }

        #region Metodos usados para la generacion de los archivos excel
        /// <summary>
        /// Obtiene un titulo acorde al semestre para ser usado en el excel de PlanifacionAcademica
        /// </summary>
        /// <param name="idSemestre">Id del semestre</param>
        /// <param name="tituloPlanificacion">Constante que se concatena con el periodo actual y semestre</param>
        /// <param name="periodoAcademico">Periodo academico actual</param>
        /// <returns>Titulo acorde al semestre pasado</returns>
        private string GetPlanifacionAcademicaReportHeaderTitle(int idSemestre, string tituloPlanificacion, string periodoAcademico)
        {
            string titulo = String.Empty;
            switch (idSemestre)
            {
                case 3:
                    titulo = tituloPlanificacion + periodoAcademico + " SEMESTRE III";
                    break;
                case 4:
                    titulo = tituloPlanificacion + periodoAcademico + " SEMESTRE IV";
                    break;
                case 5:
                    titulo = tituloPlanificacion + periodoAcademico + " SEMESTRE V";
                    break;
                case 6:
                    titulo = tituloPlanificacion + periodoAcademico + " SEMESTRE VI";
                    break;
                case 7:
                    titulo = tituloPlanificacion + periodoAcademico + " SEMESTRE VII";
                    break;
                case 8:
                    titulo = tituloPlanificacion + periodoAcademico + " SEMESTRE VIII";
                    break;
                case 9:
                    titulo = tituloPlanificacion + periodoAcademico + " SEMESTRE IX";
                    break;
                case 10:
                    titulo = tituloPlanificacion + periodoAcademico + " ELECTIVAS MAQUINAS ELECTRICAS COD-41";
                    break;
                case 11:
                    titulo = tituloPlanificacion + periodoAcademico + " ELECTIVAS CONTROLES INDUSTRIALES COD-43";
                    break;
                case 12:
                    titulo = tituloPlanificacion + periodoAcademico + " ELECTIVAS SISTEMAS DE COMUNICACION COD-44";
                    break;
                case 13:
                    titulo = tituloPlanificacion + periodoAcademico + " ELECTIVAS DE COMPUTACION COD-46";
                    break;
                case 14:
                    titulo = tituloPlanificacion + periodoAcademico + " ASIGNATURAS DEL DIS A OTRAS CARRERAS";
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
        /// <returns>Devuelve la ruta completa del archivo generado</returns>
        private string SaveExcel(ExcelPackage excel, string rootPath, string excelFileName)
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
            return outputDir;
        }

        /// <summary>
        /// Aplica algunos estilos al excel de PlanifacionAcademica
        /// </summary>
        /// <param name="worksheet">Hoja de PlanifacionAcademica</param>
        /// <param name="rowStart">Entero que indica la fila donde se empieza a aplicar los estilos</param>
        private void SetPlanifacionAcademicaReportBodyStyles(ExcelWorksheet worksheet, int rowStart)
        {
            string celda = String.Format("B{0}:I{0}", rowStart);
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
            worksheet.Row(rowStart).Height = 30;
        }

        /// <summary>
        /// Aplica algunos estilos al excel de PlanificacionHorario
        /// </summary>
        /// <param name="worksheet">Hoja de PlanifacionAcademica</param>
        /// <param name="rowStart">Entero que indica la fila donde se empieza a aplicar los estilos</param>
        /// <param name="rowEnd">Entero que indica la fila donde se termina de aplicar los estilos</param>
        private void SetPlanifacionHorarioReportBodyStyles(ExcelWorksheet worksheet, int rowStart, int rowEnd)
        {
            using (ExcelRange range = worksheet.Cells[rowStart, 2, rowEnd, 9])
            {
                range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                range.Style.WrapText = true;
                range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }
            for (int i = rowStart; i <= rowEnd; i++)
                worksheet.Row(i).Height = 30;
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
            int idPeriodo = _db.PeriodoCarreraRepository.GetCurrentPeriodo().IdPeriodo;
            var secciones = _db.SeccionesRepository.GetAllCurrent();

            if (secciones.Count() == 0)
                return;

            for (byte idPrioridad = 1; idPrioridad <= 6; idPrioridad++)
            {
                for (byte idSemestre = 3; idSemestre <= 14; idSemestre++)
                {
                    var seccionesBySemestre = secciones.Where(sec => sec.Materia.Semestre.IdSemestre == idSemestre);
                    foreach (var seccion in seccionesBySemestre)
                    {
                        var disponibilidades = _db.DisponibilidadProfesorRepository.GetByPrioridadMateria(idPrioridad, seccion.Materia.Codigo);
                        var cedulas = disponibilidades.Select(ci => ci.Cedula).Distinct();
                        int numeroSeccion = _db.HorarioProfesorRepository.GetLastSeccionAssigned(seccion.Materia.Codigo) + 1;
                        for (int i = 1; i <= seccion.NumeroSecciones; i++)
                        {
                            foreach (uint cedula in cedulas)
                            {
                                if (numeroSeccion > seccion.NumeroSecciones)
                                    break;

                                var disp = disponibilidades.Where(d => d.Cedula == cedula);
                                int horasAsignadas = _db.HorarioProfesorRepository.CalculateHorasAsignadas(cedula);
                                byte horasRestantes = (byte)(disp.FirstOrDefault().HorasACumplir - horasAsignadas);

                                if (horasRestantes < seccion.Materia.HorasAcademicasSemanales)
                                    break;

                                var aulas = _db.AulasRepository.GetByTipoCapacidad(seccion.Materia.TipoMateria.IdTipo, seccion.CantidadAlumnos);
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

            _db.HorarioProfesorRepository
                .AddRange(Mapper.Map<IEnumerable<HorarioProfesorDTO>, IEnumerable<HorarioProfesores>>(horariosGenerados));
            return _db.Save();
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

            _db.HorarioProfesorRepository
                .AddRange(Mapper.Map<IEnumerable<HorarioProfesorDTO>, IEnumerable<HorarioProfesores>>(horariosGenerados));
            return _db.Save();
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
            var horarioProfesores = _db.HorarioProfesorRepository.GetByDiaAula(idDia, idAula);
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
            var horarios = _db.HorarioProfesorRepository.GetByCedulaDia(cedula, idDia);
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
        private bool ValidateChoqueSemestre(ushort codigo, byte idSemestre, byte idHoraInicio, byte idHoraFin, byte idDia)
        {
            bool result = true;
            if (idSemestre > 9)
                return result;
            var horarios = _db.HorarioProfesorRepository.GetBySemestreDia(idSemestre, idDia);
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