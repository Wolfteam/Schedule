using System.ComponentModel.DataAnnotations;

namespace Schedule.Entities
{
    public class ProfesorBaseDTO : PersonaBase
    {
        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Apellido { get; set; }

    }
    public class ProfesorDetailsDTO : ProfesorBaseDTO
    {
        public UsuarioDTO Usuario { get; set; }
        public PrioridadProfesorDTO Prioridad { get; set; }
    }

    public class ProfesorDTO : ProfesorBaseDTO
    {
        [Required]
        public byte IdPrioridad { get; set; }
    }
}
