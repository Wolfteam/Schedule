using System.Collections.Generic;

namespace Schedule.API.Models
{
    public partial class Dias
    {
        public Dias()
        {
            DisponibilidadProfesores = new HashSet<DisponibilidadProfesores>();
            HorarioProfesores = new HashSet<HorarioProfesores>();
        }

        public byte IdDia { get; set; }
        public string NombreDia { get; set; }

        public ICollection<DisponibilidadProfesores> DisponibilidadProfesores { get; set; }
        public ICollection<HorarioProfesores> HorarioProfesores { get; set; }
    }
}
