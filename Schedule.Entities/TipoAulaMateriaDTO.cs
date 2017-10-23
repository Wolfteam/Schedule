using System.ComponentModel.DataAnnotations;

namespace Schedule.Entities
{
    public class TipoAulaMateriaDTO
    {
        public byte IdTipo { get; set; }
        
        [Required]
        public string NombreTipo { get; set; }
    }
}
