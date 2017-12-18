using Schedule.Entities;

namespace Schedule.API.Models.Repositories
{
    public interface ITokenRepository : IRepository<Tokens>
    {
        Tokens GenerateToken(string username);

        Entities.Privilegios GetAllPrivilegiosByToken(string token);

        TokenDTO Get(string token);

        ProfesorDetailsDTO GetProfesorInfoByToken(string token);

        void RemoveByCedula(int cedula);

        void RemoveByToken(string token);

        bool TokenExists(string token);
    }
}