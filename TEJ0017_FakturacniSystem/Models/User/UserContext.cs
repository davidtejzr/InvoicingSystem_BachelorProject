using Microsoft.EntityFrameworkCore;

namespace TEJ0017_FakturacniSystem.Models.User
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Purser> Pursers { get; set; }
    }
}
