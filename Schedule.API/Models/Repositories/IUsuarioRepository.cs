namespace Schedule.API.Models.Repositories
{
    public interface IUsuarioRepository : IRepository<Admin>
    {
        bool UserExists(string username, string password);

        bool IsUserAdmin(string username);
    }
}