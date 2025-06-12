using Mind_Your_Drink_Server.Models;

namespace Mind_Your_Drink_Server.Data.Repositories
{
    public interface IAdminRepository : IRepository<Admin>
    {
        Task<Admin> GetByName(string name);

        Task<bool> IsExist(string name);

        Task<Admin> GetByHash (string hash);

    }
}
