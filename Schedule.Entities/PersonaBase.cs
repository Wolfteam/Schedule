using System.ComponentModel.DataAnnotations;

namespace Schedule.Entities
{
    public class PersonaBase
    {
        [Required]
        public int Cedula { get; set; }
    }
}
