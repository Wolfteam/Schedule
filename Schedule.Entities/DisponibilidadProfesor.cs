using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Schedule.Entities
{
    public class DisponibilidadProfesor
    {
        [Required]
        public int Cedula { get; set; }

        [Required]
        public List<int> IDDias = new List<int>();

        [Required]
        public List<int> IDHoraInicio = new List<int>();

        [Required]
        public List<int> IDHoraFin = new List<int>();

        public int HorasACumplir { get; set; }
        public int HorasAsignadas { get; set; }
    }
}
