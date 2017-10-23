using System.ComponentModel.DataAnnotations;

namespace Schedule.Entities
{
    public class ProfesorMateriaBaseDTO
    {
        public uint Id { get; set; }

    }
    public class ProfesorMateriaDetailsDTO : ProfesorMateriaBaseDTO
    {
        public ProfesorDetailsDTO Profesor { get; set; }
        public MateriasDetailsDTO Materia { get; set; }
    }

    public class ProfesorMateriaDTO : ProfesorMateriaBaseDTO
    {
        [Required]
        public uint Cedula { get; set; }

        [Required]
        public ushort Codigo { get; set; }
    }

}
