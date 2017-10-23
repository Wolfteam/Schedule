using System;
using System.Collections.Generic;

namespace Schedule.API.Models
{
    public partial class Privilegios
    {
        public Privilegios()
        {
            Admin = new HashSet<Admin>();
        }

        public byte IdPrivilegio { get; set; }
        public string NombrePrivilegio { get; set; }

        public ICollection<Admin> Admin { get; set; }
    }
}
