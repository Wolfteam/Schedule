using System;
using System.ComponentModel.DataAnnotations;

namespace Schedule.Entities
{
    public class PeriodoCarreraDTO
    {
        [Key]
        public int IdPeriodo { get; set; }

        [Required]
        public string NombrePeriodo { get; set; }
        
        [Required]
        public bool Status { get; set; }
        
        [Required]
        public DateTime? FechaCreacion { get; set; }
    }
}
