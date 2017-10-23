using System;
using System.Collections.Generic;

namespace Schedule.API.Models
{
    public partial class Semestres
    {
        public Semestres()
        {
            Materias = new HashSet<Materias>();
        }

        public byte IdSemestre { get; set; }
        public string NombreSemestre { get; set; }

        public ICollection<Materias> Materias { get; set; }
    }
}
