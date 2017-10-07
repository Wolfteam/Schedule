using System.ComponentModel.DataAnnotations;

namespace Schedule.Entities
{
    public class PrioridadProfesor
    {
        [Required]
        public int ID { get; set; }

        [Required]
        public string CodigoPrioridad { get; set; }

        [Required]
        public int HorasACumplir { get; set; }
    }
}
