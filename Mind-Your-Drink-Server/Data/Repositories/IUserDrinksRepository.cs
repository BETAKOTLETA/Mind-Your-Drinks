using Mind_Your_Drink_Models.Models;

namespace Mind_Your_Drink_Models.Data.Repositories
{
    public interface IUserDrinksRepository : IRepository<UserDrink>
    {
        Task<UserDrink> GetByUserName(string name);

        Task<bool> IsExist(string name);

        Task<IEnumerable<UserDrink>> GetAllByUserId(int id);

        

    }
}
