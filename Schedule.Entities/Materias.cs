using System.ComponentModel.DataAnnotations;

namespace Schedule.Entities
{
    public class Materias
    {
        [Required]
        public int Codigo { get; set; }

        [Required]
        public string Nombre { get; set; }
        
        public Semestre Semestre { get; set; }
        public TipoAulaMateria TipoMateria { get; set; }
        public Carrera Carrera { get; set; }

        [Required]
        public int HorasAcademicasTotales { get; set; }

        [Required]
        public int HorasAcademicasSemanales { get; set; }
    }
}
