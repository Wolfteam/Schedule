using System.ComponentModel.DataAnnotations;

namespace Schedule.Entities
{
    public class SemestreDTO
    {
        [Required]
        public byte IdSemestre { get; set; }

        [Required]
        public string NombreSemestre { get; set; }
    }
}
