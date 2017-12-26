using System.ComponentModel.DataAnnotations;

namespace Schedule.Entities
{
    public enum Privilegios
    {
        Profesor = 1,
        Administrador = 2
    }

    public class UsuarioBaseDTO : PersonaBase
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class UsuarioDTO : UsuarioBaseDTO
    {
        [Required]
        public byte IdPrivilegio { get; set; }
    }

    public class UsuarioDetailsDTO : UsuarioBaseDTO
    {
        public ProfesorDTO Profesor { get; set; }
        public PrivilegiosDTO Privilegios { get; set; }
    }
}
