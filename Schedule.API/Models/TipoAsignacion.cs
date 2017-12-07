using System;
using System.Collections.Generic;

namespace Schedule.API.Models
{
    public partial class TipoAsignacion
    {
        public TipoAsignacion()
        {
            HorarioProfesores = new HashSet<HorarioProfesores>();
        }

        public int IdAsignacion { get; set; }
        public string NombreAsignacion { get; set; }

        public ICollection<HorarioProfesores> HorarioProfesores { get; set; }
    }
}
