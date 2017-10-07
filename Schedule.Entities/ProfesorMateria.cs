namespace Schedule.Entities
{
    public class ProfesorMateria
    {
        public int ID { get; set; }
        public Profesor Profesor { get; set; }
        public Materias Materia { get; set; }
    }
}
