using System.ComponentModel.DataAnnotations;

namespace Schedule.Entities
{
    public class Secciones
    {
        public Materias Materia { get; set; }

        [Required]
        public int NumeroSecciones { get; set; }
        
        [Required]
        public int CantidadAlumnos { get; set; }
    }
}
