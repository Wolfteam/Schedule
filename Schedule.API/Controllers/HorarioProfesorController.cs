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
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        private readonly IHostingEnvironment _hostingEnvironment;
        public HorarioProfesorController(IHostingEnvironment environment)
        {
            _hostingEnvironment = environment;
        }

        [HttpGet("Generate")]
        public IActionResult Generate(bool fromStart)
        {
            // bool recordsExists = _unitOfWork.HorarioProfesor.RecordsExists();
            // if (!recordsExists)
            // {
            //     Generate();
            // }
            // return Ok();
            ReadExcel();
            //CreateExcel();
            return Ok();
        }
        //TODO: Dividir modelo de excel en varias hojas?
        private void CreateExcel()
        {
            string path = _hostingEnvironment.ContentRootPath;
            var newFile = new FileInfo(path + @"\sample1.xlsx");
            if (newFile.Exists)
            {
                newFile.Delete();  // ensures we create a new workbook
                newFile = new FileInfo(path + @"\sample1.xlsx");
            }
            using (var package = new ExcelPackage(newFile))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Inventory");
                //Add the headers
                worksheet.Cells[1, 1].Value = "ID";
                worksheet.Cells[1, 2].Value = "Product";
                worksheet.Cells[1, 3].Value = "Quantity";
                worksheet.Cells[1, 4].Value = "Price";
                worksheet.Cells[1, 5].Value = "Value";

                //Add some items...
                worksheet.Cells["A2"].Value = 12001;
                worksheet.Cells["B2"].Value = "Nails";
                worksheet.Cells["C2"].Value = 37;
                worksheet.Cells["D2"].Value = 3.99;

                worksheet.Cells["A3"].Value = 12002;
                worksheet.Cells["B3"].Value = "Hammer";
                worksheet.Cells["C3"].Value = 5;
                worksheet.Cells["D3"].Value = 12.10;

                worksheet.Cells["A4"].Value = 12003;
                worksheet.Cells["B4"].Value = "Saw";
                worksheet.Cells["C4"].Value = 12;
                worksheet.Cells["D4"].Value = 15.37;

                //Add a formula for the value-column
                worksheet.Cells["E2:E4"].Formula = "C2*D2";

                //Ok now format the values;
                using (var range = worksheet.Cells[1, 1, 1, 5])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                    range.Style.Font.Color.SetColor(Color.White);
                }

                worksheet.Cells["A5:E5"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A5:E5"].Style.Font.Bold = true;

                worksheet.Cells[5, 3, 5, 5].Formula = string.Format("SUBTOTAL(9,{0})", new ExcelAddress(2, 3, 4, 3).Address);
                worksheet.Cells["C2:C5"].Style.Numberformat.Format = "#,##0";
                worksheet.Cells["D2:E5"].Style.Numberformat.Format = "#,##0.00";

                //Create an autofilter for the range
                worksheet.Cells["A1:E4"].AutoFilter = true;

                worksheet.Cells["A2:A4"].Style.Numberformat.Format = "@";   //Format as text

                //There is actually no need to calculate, Excel will do it for you, but in some cases it might be useful. 
                //For example if you link to this workbook from another workbook or you will open the workbook in a program that hasn't a calculation engine or 
                //you want to use the result of a formula in your program.
                worksheet.Calculate();

                worksheet.Cells.AutoFitColumns(0);  //Autofit columns for all cells

                // lets set the header text 
                worksheet.HeaderFooter.OddHeader.CenteredText = "&24&U&\"Arial,Regular Bold\" Inventory";
                // add the page number to the footer plus the total number of pages
                worksheet.HeaderFooter.OddFooter.RightAlignedText =
                    string.Format("Page {0} of {1}", ExcelHeaderFooter.PageNumber, ExcelHeaderFooter.NumberOfPages);
                // add the sheet name to the footer
                worksheet.HeaderFooter.OddFooter.CenteredText = ExcelHeaderFooter.SheetName;
                // add the file path to the footer
                worksheet.HeaderFooter.OddFooter.LeftAlignedText = ExcelHeaderFooter.FilePath + ExcelHeaderFooter.FileName;

                worksheet.PrinterSettings.RepeatRows = worksheet.Cells["1:2"];
                worksheet.PrinterSettings.RepeatColumns = worksheet.Cells["A:G"];

                // Change the sheet view to show it in page layout mode
                //worksheet.View.PageLayoutView = true;

                // set some document properties
                package.Workbook.Properties.Title = "Invertory";
                package.Workbook.Properties.Author = "Jan Källman";
                package.Workbook.Properties.Comments = "This sample demonstrates how to create an Excel 2007 workbook using EPPlus";

                // set some extended property values
                package.Workbook.Properties.Company = "AdventureWorks Inc.";

                // set some custom property values
                package.Workbook.Properties.SetCustomPropertyValue("Checked by", "Jan Källman");
                package.Workbook.Properties.SetCustomPropertyValue("AssemblyName", "EPPlus");
                // save our new workbook and we are done!
                package.Save();
            }
            //return newFile.FullName;
        }

        public void ReadExcel()
        {
            string path = _hostingEnvironment.ContentRootPath;
            var modelDir = new FileInfo(path + @"\wwwroot\Resources\ModeloPlanificacion.xlsx");

            using (var excel = new ExcelPackage(modelDir))
            {
                ExcelWorksheet planificacion = excel.Workbook.Worksheets[0];
                planificacion.Cells["B3"].Value = 44023;
                planificacion.Cells["C3"].Value = "Sistemas de Comunicacion";
                planificacion.Cells["D3"].Value = "Alexander Tinoco";
                planificacion.Cells["E3"].Value = 1;
                planificacion.Cells["F3"].Value = "Lunes";
                planificacion.Cells["G3"].Value = "7:00 a 8:40 am";
                planificacion.Cells["H3"].Value = 2412;
                planificacion.Cells["I3"].Value = 30;
                using (ExcelRange range = planificacion.Cells["B3:I3"])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
                    range.Style.Font.Color.SetColor(Color.White);
                    range.Style.Font.Bold = true;
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }
                //Esto copia varias files y columnas a otra posicion
                //Cells[RowStart, ColumnStart, RowEnd, ColumnEnd ]
                planificacion.Cells[1,2, 2, 9].Copy(planificacion.Cells[5, 2, 7, 9]);
                //Esto le hace un autofit a toda la hoja
                planificacion.Cells[planificacion.Dimension.Address].AutoFitColumns();
                string outputDir = path + @"\sample4.xlsx";
                FileInfo x = new FileInfo(outputDir);
                if (x.Exists)
                    x.Delete();
                excel.SaveAs(x);
            }
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
    }
}