using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api.Models
{
    public class CredentialsContext : IdentityDbContext<IdentityUser>
    {
        public CredentialsContext(DbContextOptions<CredentialsContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}