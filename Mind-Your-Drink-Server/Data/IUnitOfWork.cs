using Mind_Your_Drink_Server.Data.Repositories;

namespace Mind_Your_Drink_Server.Data
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IAdminRepository Admins { get; }

        int Complete();
        Task<int> CompleteAsync();
    }
}
