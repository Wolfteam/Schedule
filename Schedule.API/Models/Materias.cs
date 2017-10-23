using System.Collections.Generic;

namespace Schedule.API.Models
{
    public partial class Materias
    {
        public Materias()
        {
            ProfesoresMaterias = new HashSet<ProfesoresMaterias>();
        }

        public ushort Codigo { get; set; }
        public string Asignatura { get; set; }
        public byte IdSemestre { get; set; }
        public byte IdTipo { get; set; }
        public byte IdCarrera { get; set; }
        public byte HorasAcademicasTotales { get; set; }
        public byte HorasAcademicasSemanales { get; set; }

        public Carreras Carreras { get; set; }
        public Semestres Semestres { get; set; }
        public TipoAulaMaterias TipoAulaMaterias { get; set; }
        public Secciones Secciones { get; set; }
        public ICollection<ProfesoresMaterias> ProfesoresMaterias { get; set; }
    }
}
