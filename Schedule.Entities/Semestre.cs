using System.ComponentModel.DataAnnotations;

namespace Schedule.Entities
{
    public class Semestre
    {
        [Required]
        public int ID { get; set; }

        [Required]
        public string NombreSemestre { get; set; }
    }
}
