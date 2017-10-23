using System.ComponentModel.DataAnnotations;

namespace Schedule.Entities
{
    public class MateriasBaseDTO
    {
        [Required]
        public ushort Codigo { get; set; }

        [Required]
        public string Asignatura { get; set; }

        [Required]
        public byte HorasAcademicasTotales { get; set; }

        [Required]
        public byte HorasAcademicasSemanales { get; set; }
    }

    public class MateriasDetailsDTO : MateriasBaseDTO
    {
        public SemestreDTO Semestre { get; set; }
        public TipoAulaMateriaDTO TipoMateria { get; set; }
        public CarreraDTO Carrera { get; set; }
    }

    public class MateriasDTO : MateriasBaseDTO
    {
        [Required]
        public byte IdSemestre { get; set; }

        [Required]
        public byte IdTipo { get; set; }

        [Required]
        public byte IdCarrera { get; set; }
    }

}
