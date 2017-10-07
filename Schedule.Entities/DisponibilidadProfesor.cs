using System.ComponentModel.DataAnnotations;

namespace Schedule.Entities
{
    public class DisponibilidadProfesor
    {
        [Required]
        public int Cedula { get; set; }

        [Required]
        public DiasHabiles Dia { get; set; }

        [Required]
        public Horas HoraInicio { get; set; }

        [Required]
        public Horas HoraFin { get; set; }
    }
}
