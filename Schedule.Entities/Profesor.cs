using System.ComponentModel.DataAnnotations;

namespace Schedule.Entities
{
    public class Profesor : PersonaBase
    {
        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Apellido { get; set; }

        public Usuario Usuario { get; set; }
        public PrioridadProfesor Prioridad { get; set; }
    }
}
