using System;
using System.Collections.Generic;

namespace Schedule.API.Models
{
    public partial class PeriodoCarrera
    {
        public PeriodoCarrera()
        {
            DisponibilidadProfesores = new HashSet<DisponibilidadProfesores>();
            HorarioProfesores = new HashSet<HorarioProfesores>();
            Secciones = new HashSet<Secciones>();
        }

        public int IdPeriodo { get; set; }
        public string NombrePeriodo { get; set; }
        public bool Status { get; set; }
        public DateTime? FechaCreacion { get; set; }

        public ICollection<DisponibilidadProfesores> DisponibilidadProfesores { get; set; }
        public ICollection<HorarioProfesores> HorarioProfesores { get; set; }
        public ICollection<Secciones> Secciones { get; set; }
    }
}
