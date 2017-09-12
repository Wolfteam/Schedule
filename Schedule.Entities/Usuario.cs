using System;
using System.Collections.Generic;
using System.Text;

namespace Schedule.Entities
{
    class Usuario
    {
        public enum Privilegios
        {
            Profesor = 1,
            Administrador = 2
        }

        public int Cedula { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Privilegios IdPrivilegios { get; set; }
    }
}
