namespace Schedule.Entities
{
    public class Materias
    {
        public int Codigo { get; set; }
        public string Nombre { get; set; }
        public Semestre Semestre { get; set; }
        public TipoAulaMateria TipoMateria { get; set; }
        public Carrera Carrera { get; set; }
        public int HorasAcademicasTotales { get; set; }
        public int HorasAcademicasSemanales { get; set; }
    }
}
