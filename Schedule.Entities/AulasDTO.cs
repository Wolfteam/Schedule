using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Schedule.Entities
{
    public class AulasBaseDTO
    {
        [Key]
        public byte IdAula { get; set; }

        [Required]
        public string NombreAula { get; set; }

        [Required]
        public byte Capacidad { get; set; }
    }

    public class AulasDetailsDTO : AulasBaseDTO
    {
        public TipoAulaMateriaDTO TipoAula { get; set; }
    }

    public class AulasDTO : AulasBaseDTO
    {
        [Required]
        public byte IdTipo { get; set; }
    }
}
