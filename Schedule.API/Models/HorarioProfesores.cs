namespace Schedule.API.Models
{
    public partial class HorarioProfesores
    {
        public uint Cedula { get; set; }
        public ushort Codigo { get; set; }
        public byte IdDia { get; set; }
        public byte IdHoraInicio { get; set; }
        public byte IdHoraFin { get; set; }
        public byte IdAula { get; set; }
        public byte NumeroSeccion { get; set; }
        public int IdPeriodo { get; set; }

        public Profesores Profesores { get; set; }
        public Secciones Secciones { get; set; }
        public Aulas Aulas { get; set; }
        public Dias Dias { get; set; }
        public Horas HoraFin { get; set; }
        public Horas HoraInicio { get; set; }
        public PeriodoCarrera PeriodoCarrera { get; set; }
    }
}
