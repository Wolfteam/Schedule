using System;
using System.Collections.Generic;

namespace Schedule.API.Models
{
    public partial class PrioridadProfesor
    {
        public PrioridadProfesor()
        {
            Profesores = new HashSet<Profesores>();
        }

        public byte IdPrioridad { get; set; }
        public string CodigoPrioridad { get; set; }
        public byte HorasACumplir { get; set; }

        public ICollection<Profesores> Profesores { get; set; }
    }
}
