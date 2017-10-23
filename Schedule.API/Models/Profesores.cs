using System;
using System.Collections.Generic;

namespace Schedule.API.Models
{
    public partial class Profesores
    {
        public Profesores()
        {
            DisponibilidadProfesores = new HashSet<DisponibilidadProfesores>();
            HorarioProfesores = new HashSet<HorarioProfesores>();
            ProfesoresMaterias = new HashSet<ProfesoresMaterias>();
        }

        public uint Cedula { get; set; }
        public string Nombre { get; set; }
        public string Nombre2 { get; set; }
        public string Apellido { get; set; }
        public string Apellido2 { get; set; }
        public byte IdPrioridad { get; set; }

        public PrioridadProfesor PrioridadProfesor { get; set; }
        public Admin Admin { get; set; }
        public ICollection<DisponibilidadProfesores> DisponibilidadProfesores { get; set; }
        public ICollection<HorarioProfesores> HorarioProfesores { get; set; }
        public ICollection<ProfesoresMaterias> ProfesoresMaterias { get; set; }
    }
}
