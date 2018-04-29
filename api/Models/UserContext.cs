using Microsoft.EntityFrameworkCore;

namespace api.Models
{
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        
        public UserContext(DbContextOptions<UserContext> options) : base(options) {}
    }
}