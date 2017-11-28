using System.ComponentModel.DataAnnotations;

namespace Schedule.Entities
{
    public class SeccionesBaseDTO
    {
        [Required]
        public byte NumeroSecciones { get; set; }

        [Required]
        public byte CantidadAlumnos { get; set; }
    }

    public class SeccionesDetailsDTO : SeccionesBaseDTO
    {
        public MateriasDetailsDTO Materia { get; set; }
        public PeriodoCarreraDTO PeriodoCarrera { get; set; }
    }

    public class SeccionesDTO : SeccionesBaseDTO
    {
        [Required]
        public ushort Codigo { get; set; }
        
        public int IdPeriodo { get; set; }
    }

}
