using System.ComponentModel.DataAnnotations;

namespace Schedule.Entities
{
    public class PrioridadProfesorDTO
    {
        [Required]
        public byte ID { get; set; }

        [Required]
        public string CodigoPrioridad { get; set; }

        [Required]
        public byte HorasACumplir { get; set; }
    }
}
