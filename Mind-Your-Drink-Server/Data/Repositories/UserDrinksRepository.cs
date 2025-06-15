using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Mind_Your_Drink_Models.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Mind_Your_Drink_Models.Data.Repositories
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


        public async Task<IEnumerable<UserDrink>> GetByDayByUserIdAsync(int id, DateTime date)
        {
            var nextDay = date.Date.AddDays(1);

            return await _context.UserDrinks
                .Where(d => d.UserId == id && d.Time >= date.Date && d.Time < nextDay)
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