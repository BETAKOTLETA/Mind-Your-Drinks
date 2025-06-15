using Microsoft.EntityFrameworkCore;
using Mind_Your_Drink_Server.Models;

namespace Mind_Your_Drink_Server.Data.Repositories
{
    public class UserDrinksRepository : Repository<UserDrink>, IUserDrinksRepository
    {
        public UserDrinksRepository(MindDrinkDBContext context) : base(context)
        {
        }

        public async Task<IEnumerable<UserDrink>> GetAllByUserId(int id)
        {
            return await _context.UserDrinks
            .Where(ud => ud.UserId == id)
            .ToListAsync();
        }

        //public async Task<User> GetByName(string name)
        //{
        //    return await _context.Users.FirstOrDefaultAsync(u => u.Name == name);
        //}

        public async Task<UserDrink> GetByUserName(string name)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Name == name);
            if (user == null)
                return null;
            return await _context.UserDrinks.FirstOrDefaultAsync(x => x.UserId == user.Id);
            
        }

        public async Task<bool> IsExist(string name)
        {
            return await _context.Users.AnyAsync(u => u.Name == name);
        }


    }
}