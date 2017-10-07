using System.ComponentModel.DataAnnotations;

namespace Schedule.Entities
{
    public class Horas
    {
        public int ID { get; set; }

        [Required]
        public string Hora { get; set; }
    }
}
