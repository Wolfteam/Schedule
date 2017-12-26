using System.ComponentModel.DataAnnotations;

namespace Schedule.Entities
{
    public class PrivilegiosDTO
    {
        [Key]
        public byte IdPrivilegio { get; set; }

        [Required]
        public string NombrePrivilegio { get; set; }
    }
}
