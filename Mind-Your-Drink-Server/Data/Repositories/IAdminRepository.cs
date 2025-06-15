using Mind_Your_Drink_Models.Models;

namespace Mind_Your_Drink_Models.Data.Repositories
{
    public interface IAdminRepository : IRepository<Admin>
    {
        Task<Admin> GetByName(string name);

        Task<bool> IsExist(string name);

        Task<Admin> GetByHash (string hash);

    }
}
