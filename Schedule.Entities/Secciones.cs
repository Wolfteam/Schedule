using System.ComponentModel.DataAnnotations;

namespace Schedule.Entities
{
    public class Secciones
    {
        [Required]
        public int CodigoMateria { get; set; }

        [Required]
        public int NumeroSecciones { get; set; }
        
        [Required]
        public int CantidadAlumnos { get; set; }
    }
}
