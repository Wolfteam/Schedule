using System.ComponentModel.DataAnnotations;

namespace Schedule.Entities
{
    public class HorarioProfesor : DisponibilidadProfesorDTO
    {
        [Required]
        public int Codigo { get; set; }

        [Required]
        public AulasDetailsDTO Aula { get; set; }
        
        [Required]
        public SeccionesDetailsDTO Seccion { get; set; }
    }
}
