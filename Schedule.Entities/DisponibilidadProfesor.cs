namespace Schedule.Entities
{
    public class DisponibilidadProfesor
    {
        public int Cedula { get; set; }
        public DiasHabiles Dia { get; set; }
        public Horas HoraInicio { get; set; }
        public Horas HoraFin { get; set; }
    }
}
