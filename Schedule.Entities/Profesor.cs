using System;

namespace Schedule.Entities
{
    public enum PrioridadID
    {
        Contratado_Horas = 1,
        Contratado_Medio_Tiempo = 2,
        Contratado_Tiempo_Completo = 3,
        Medio_Tiempo = 4,
        Tiempo_Completo = 5,
        Dedicacion_Exclusiva = 6
    }
    public class Profesor : Usuario
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public PrioridadID PrioridadProfesor { get; set; }
    }
}
