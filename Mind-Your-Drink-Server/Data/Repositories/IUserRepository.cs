using Mind_Your_Drink_Server.Models;

namespace Mind_Your_Drink_Server.Data.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByName(string name);

        Task<bool> IsExist(string name);

        
    }
}
