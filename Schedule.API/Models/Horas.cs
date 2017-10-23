using System;
using System.Collections.Generic;

namespace Schedule.API.Models
{
    public partial class Horas
    {
        public Horas()
        {
            DisponibilidadProfesoresIdHoraFinNavigation = new HashSet<DisponibilidadProfesores>();
            DisponibilidadProfesoresIdHoraInicioNavigation = new HashSet<DisponibilidadProfesores>();
            HorarioProfesoresIdHoraFinNavigation = new HashSet<HorarioProfesores>();
            HorarioProfesoresIdHoraInicioNavigation = new HashSet<HorarioProfesores>();
        }

        public byte IdHora { get; set; }
        public string NombreHora { get; set; }

        public ICollection<DisponibilidadProfesores> DisponibilidadProfesoresIdHoraFinNavigation { get; set; }
        public ICollection<DisponibilidadProfesores> DisponibilidadProfesoresIdHoraInicioNavigation { get; set; }
        public ICollection<HorarioProfesores> HorarioProfesoresIdHoraFinNavigation { get; set; }
        public ICollection<HorarioProfesores> HorarioProfesoresIdHoraInicioNavigation { get; set; }
    }
}
