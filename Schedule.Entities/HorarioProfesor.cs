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
}
