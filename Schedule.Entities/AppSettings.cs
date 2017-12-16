namespace Schedule.Entities
{
    public class AppSettings
    {
        public string URLBaseAPI { get; set; }
        public string URLBase { get; set; }
        public string ConnectionString { get; set; }
        public ExcelHorarioProfesorSettings ExcelHorarioProfesorSettings { get; set; }
    }

    public class ExcelHorarioProfesorSettings
    {
        public string ModeloPlanificacionPath { get; set; }
        public string PlanificacionAcademicaExcelFileName { get; set; }
        public string PlanificacionAulasExcelFileName { get; set; }
        public string PlanificacionHorariosExcelFileName { get; set; }
        public string SavePath { get; set; }  
    }
}