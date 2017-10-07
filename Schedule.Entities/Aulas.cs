using System.ComponentModel.DataAnnotations;

namespace Schedule.Entities
{
    public class Aulas
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public int Capacidad { get; set; }
        
        [Required]
        public int IDTipoAula { get; set; }
    }
}
