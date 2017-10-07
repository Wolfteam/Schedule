using System.ComponentModel.DataAnnotations;

namespace Schedule.Entities
{
    public class Carrera
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string NombreCarrera { get; set; }
    }
}
