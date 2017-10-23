using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Schedule.Entities
{
    public class DisponibilidadProfesorDTO
    {
        [Required]
        public uint Cedula { get; set; }

        [Required]
        public List<byte> IDDias = new List<byte>();

        [Required]
        public List<byte> IDHoraInicio = new List<byte>();

        [Required]
        public List<byte> IDHoraFin = new List<byte>();

        public byte HorasACumplir { get; set; }
        public int HorasAsignadas { get; set; }
    }
}
