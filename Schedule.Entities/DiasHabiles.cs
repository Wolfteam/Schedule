using System.ComponentModel.DataAnnotations;

namespace Schedule.Entities
{
    public class DiasHabiles
    {
        [Key]
        public int ID { get; set; }
        
        [Required]
        public string NombreDia { get; set; }
    }
}
