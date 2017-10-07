using System.ComponentModel.DataAnnotations;

namespace Schedule.Entities
{
    public class TipoAulaMateria
    {
        public int ID { get; set; }
        
        [Required]
        public string Nombre { get; set; }
    }
}
