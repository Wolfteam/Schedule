namespace Schedule.API.Models
{
    public partial class DisponibilidadProfesores
    {
        public uint Cedula { get; set; }
        public byte IdDia { get; set; }
        public byte IdHoraInicio { get; set; }
        public byte IdHoraFin { get; set; }

        public Profesores Profesores { get; set; }
        public Dias Dias { get; set; }
        public Horas HoraFin { get; set; }
        public Horas HoraInicio { get; set; }
    }
}
