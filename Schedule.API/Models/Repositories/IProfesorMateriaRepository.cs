using Schedule.Entities;

namespace Schedule.API.Models.Repositories
{
    public interface IProfesorMateriaRepository : IRepository<ProfesoresMaterias>
    {
        ProfesorMateriaDetailsDTO Get(uint cedula, int codigo);
    }
}