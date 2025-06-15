using Microsoft.EntityFrameworkCore;
using Mind_Your_Drink_Models.Models;

namespace Mind_Your_Drink_Models.Data.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByName(string name);

        Task<bool> IsExist(string name);

        Task<IEnumerable<User>> GetAllUsers();


    }
}
