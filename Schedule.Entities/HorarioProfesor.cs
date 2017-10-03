namespace Schedule.Entities
{
    public class HorarioProfesor : DisponibilidadProfesor
    {
        public int Codigo { get; set; }
        public int IDAula { get; set; }
        public int NumeroSeccion { get; set; }
    }
}
