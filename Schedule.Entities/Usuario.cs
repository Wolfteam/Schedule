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
    public class Usuario
    {
        [Key]
        public int Cedula { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Privilegios IdPrivilegios { get; set; }
    }
}
