using System.ComponentModel.DataAnnotations;

namespace Schedule.Entities
{
    /// <summary>
    /// Indica si el horario generado es automatico o random
    /// </summary>
    public enum Asignacion
    {
        Automatica = 1,
        Random = 2
    }

    public class HorarioProfesorDTO : PersonaBase
    {
        [Required]
        public ushort Codigo { get; set; }

        [Required]
        public byte IdDia { get; set; }

        [Required]
        public byte IdHoraInicio { get; set; }

        [Required]
        public byte IdHoraFin { get; set; }

        [Required]
        public byte IdAula { get; set; }

        [Required]
        public byte NumeroSeccion { get; set; }

        [Required]
        public int IdPeriodo { get; set; }

        [Required]
        public int IdTipoAsignacion { get; set; }
    }

    public class HorarioProfesorDetailsDTO : PersonaBase
    {
        public string Asignatura { get; set; }
        public string Aula { get; set; }
        public int CantidadAlumnos { get; set; }
        public int Codigo { get; set; }
        public string Dia { get; set; }
        public string HoraInicio { get; set; }
        public string HoraFin { get; set; }
        public int NumeroSeccion { get; set; }
        public int Prioridad { get; set; }
        public string Profesor { get; set; }
        public int Semestre { get; set; }
    }
}
