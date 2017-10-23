using System.Collections.Generic;

namespace Schedule.API.Models
{
    public partial class Secciones
    {
        public Secciones()
        {
            HorarioProfesores = new HashSet<HorarioProfesores>();
        }

        public ushort Codigo { get; set; }
        public byte NumeroSecciones { get; set; }
        public byte CantidadAlumnos { get; set; }

        public Materias Materias { get; set; }
        public ICollection<HorarioProfesores> HorarioProfesores { get; set; }
    }
}
