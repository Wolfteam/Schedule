namespace Schedule.Entities
{
    public class Materias
    {
        public int Codigo { get; set; }
        public string Nombre { get; set; }
        public int IDSemestre { get; set; }
        public int IDTipoMateria { get; set; }
        public int IDCarrera { get; set; }
        public int HorasAcademicasTotales { get; set; }
        public int HorasAcademicasSemanales { get; set; }
    }
}
