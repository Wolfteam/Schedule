using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Schedule.Entities
{
    public enum Privilegios
    {
        Profesor = 1,
        Administrador = 2
    }
    public class UsuarioDTO : PersonaBase
    {
        [Required]
        public string Username { get; set; }
        
        [Required]
        public string Password { get; set; }
        public Privilegios IdPrivilegio { get; set; }
    }
}
