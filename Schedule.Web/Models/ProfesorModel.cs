using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Schedule.Web.Models
{
    public class ProfesorModel
    {
        [Key]
        public int Cedula { get; set; }
        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public string PrimerApellido { get; set; }
        public int IdPrioridad { get; set; }
    }
}
