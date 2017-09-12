using System;

namespace Schedule.Entities
{
    public class Profesor
    {
        public int Cedula { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public Prioridad.PrioridadID IdPrioridad { get; set; }
    }
}
