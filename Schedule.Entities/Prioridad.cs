using System;
using System.Collections.Generic;
using System.Text;

namespace Schedule.Entities
{
    public class Prioridad
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
    }
}
