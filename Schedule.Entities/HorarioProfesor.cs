namespace Schedule.Entities
{
    public class HorarioProfesor : DisponibilidadProfesor
    {
        public int Codigo { get; set; }
        public Aulas Aula { get; set; }
        public Secciones Seccion { get; set; }
    }
}
