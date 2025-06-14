using Microsoft.EntityFrameworkCore;
using Mind_Your_Drink_Server.Models;
using Microsoft.EntityFrameworkCore.Design;

namespace Mind_Your_Drink_Server.Data
{
    public class MindDrinkDBContext : DbContext
    {
        public MindDrinkDBContext(DbContextOptions<MindDrinkDBContext> options) : base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<UserDrink> UserDrinks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
