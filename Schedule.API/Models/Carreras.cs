using System.Collections.Generic;

namespace Schedule.API.Models
{
    public partial class Carreras
    {
        public Carreras()
        {
            Materias = new HashSet<Materias>();
        }

        public byte IdCarrera { get; set; }
        public string NombreCarrera { get; set; }

        public ICollection<Materias> Materias { get; set; }
    }
}
