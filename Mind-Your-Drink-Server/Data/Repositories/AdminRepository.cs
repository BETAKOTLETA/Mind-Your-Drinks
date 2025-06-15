using Microsoft.EntityFrameworkCore;
using Mind_Your_Drink_Models.Models;

namespace Mind_Your_Drink_Models.Data.Repositories
{
    public class AdminRepository : Repository<Admin>, IAdminRepository
    {
        public AdminRepository(MindDrinkDBContext context) : base(context)
        {
        }

        public async Task<Admin> GetByHash(string hash)
        {
            return await _context.Admins.FirstOrDefaultAsync(u => u.HashPassword == hash);
        }

        public async Task<Admin> GetByName(string name)
        {
            return await _context.Admins.FirstOrDefaultAsync(u => u.Name == name);
        }

        public async Task<bool> IsExist(string name)
        {
            return await _context.Admins.AnyAsync(u => u.Name == name);
        }


    }
}
