using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Schedule.Entities
{
    public class DisponibilidadProfesorDTO : PersonaBase
    {
        [Required]
        public byte IdDia { get; set; }
        [Required]
        public byte IdHoraInicio { get; set; }
        [Required]
        public byte IdHoraFin { get; set; }
        
        public int IdPeriodo { get; set; }
    }

    public class DisponibilidadProfesorDetailsDTO : PersonaBase
    {
        public IEnumerable<DisponibilidadProfesorDTO> Disponibilidad { get; set; }
        public byte HorasACumplir { get; set; }
        public int HorasAsignadas { get; set; }
    }

    // public class DisponibilidadProfesorDTO
    // {
    //     [Required]
    //     public uint Cedula { get; set; }

    //     [Required]
    //     public List<byte> IDDias = new List<byte>();

    //     [Required]
    //     public List<byte> IDHoraInicio = new List<byte>();

    //     [Required]
    //     public List<byte> IDHoraFin = new List<byte>();

    //     public byte HorasACumplir { get; set; }
    //     public int HorasAsignadas { get; set; }
    // }
}
