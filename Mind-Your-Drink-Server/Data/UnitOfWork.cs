using Mind_Your_Drink_Models.Data.Repositories;

namespace Mind_Your_Drink_Models.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MindDrinkDBContext _context;

        public UnitOfWork(MindDrinkDBContext context)
        {
            _context = context;
            Users = new UserRepository(_context);
            Admins = new AdminRepository(_context);
            UserDrinks = new UserDrinksRepository(_context);
        }

        public IUserRepository Users { get; private set; }
        public IAdminRepository Admins { get; private set; }
        public IUserDrinksRepository UserDrinks { get; private set; }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

    }
}
