using System.ComponentModel.DataAnnotations;

namespace Schedule.Entities
{
    public class PersonaBase
    {
        [Required]
        public uint Cedula { get; set; }
    }
}
