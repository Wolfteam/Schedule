using System;
using System.Collections.Generic;

namespace Schedule.API.Models
{
    public partial class Admin
    {
        public Admin()
        {
            Tokens = new HashSet<Tokens>();
        }

        public uint Cedula { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public byte IdPrivilegio { get; set; }

        public Profesores CedulaNavigation { get; set; }
        public Privilegios IdPrivilegioNavigation { get; set; }
        public ICollection<Tokens> Tokens { get; set; }
    }
}
