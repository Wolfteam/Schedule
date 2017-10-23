using System;
using System.Collections.Generic;

namespace Schedule.API.Models
{
    public partial class TipoAulaMaterias
    {
        public TipoAulaMaterias()
        {
            Aulas = new HashSet<Aulas>();
            Materias = new HashSet<Materias>();
        }

        public byte IdTipo { get; set; }
        public string NombreTipo { get; set; }

        public ICollection<Aulas> Aulas { get; set; }
        public ICollection<Materias> Materias { get; set; }
    }
}
