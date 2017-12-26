namespace Schedule.API.Models.Repositories
{
    public interface IProfesorRepository : IRepository<Profesores>
    {
        byte GetHorasACumplir(int cedula);
    }
}