using System;
using System.Collections.Generic;

namespace Schedule.API.Models
{
    public partial class PeriodoCarrera
    {
        public int IdPeriodo { get; set; }
        public string NombrePeriodo { get; set; }
        public bool Status { get; set; }
        public DateTime? FechaCreacion { get; set; }
    }
}
