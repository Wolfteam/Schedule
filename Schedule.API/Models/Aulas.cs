using System.Collections.Generic;

namespace Schedule.API.Models
{
    public partial class Aulas
    {
        public Aulas()
        {
            HorarioProfesores = new HashSet<HorarioProfesores>();
        }

        public byte IdAula { get; set; }
        public string NombreAula { get; set; }
        public byte Capacidad { get; set; }
        public byte IdTipo { get; set; }

        public TipoAulaMaterias TipoAulaMateria { get; set; }
        public ICollection<HorarioProfesores> HorarioProfesores { get; set; }
    }
}
