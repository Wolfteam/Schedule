namespace Schedule.API.Models
{
    public partial class ProfesoresMaterias
    {
        public uint Id { get; set; }
        public uint Cedula { get; set; }
        public ushort Codigo { get; set; }

        public Profesores Profesores { get; set; }
        public Materias Materias { get; set; }
    }
}
