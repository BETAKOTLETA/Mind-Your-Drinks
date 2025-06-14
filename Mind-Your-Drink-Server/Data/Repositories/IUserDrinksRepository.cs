using Mind_Your_Drink_Server.Models;

namespace Mind_Your_Drink_Server.Data.Repositories
{
    public interface IUserDrinksRepository : IRepository<UserDrink>
    {
        Task<UserDrink> GetByUserName(string name);

        Task<bool> IsExist(string name);

        

    }
}
