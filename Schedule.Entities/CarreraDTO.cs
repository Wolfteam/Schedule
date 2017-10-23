using System.ComponentModel.DataAnnotations;

namespace Schedule.Entities
{
    public class CarreraDTO
    {
        [Key]
        public byte IdCarrera { get; set; }

        [Required]
        public string NombreCarrera { get; set; }
    }
}
