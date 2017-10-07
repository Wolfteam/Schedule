using System.ComponentModel.DataAnnotations;

namespace Schedule.Entities
{
    public class HorarioProfesor : DisponibilidadProfesor
    {
        [Required]
        public int Codigo { get; set; }

        [Required]
        public Aulas Aula { get; set; }
        
        [Required]
        public Secciones Seccion { get; set; }
    }
}
