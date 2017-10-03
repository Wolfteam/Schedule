namespace Schedule.Entities
{
    public class Profesor : Usuario
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public PrioridadProfesor Prioridad { get; set; }
    }
}
