using Microsoft.EntityFrameworkCore;
using Mind_Your_Drink_Server.Models;

namespace Mind_Your_Drink_Server.Data.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
            public UserRepository(MindDrinkDBContext context) : base(context)
            {
            }

            public async Task<User> GetByName(string name)
            {
                return await _context.Users.FirstOrDefaultAsync(u => u.Name == name);
            }

            public async Task<bool> IsExist(string name)
            {
                return await _context.Users.AnyAsync(u => u.Name == name);
            }
        }
    }
