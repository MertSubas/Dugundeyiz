using Dugundeyiz.Entity;
using Dugundeyiz.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Dugundeyiz.Context
{
    public class DugundeyizContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public DugundeyizContext(DbContextOptions<DugundeyizContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Category> Categories { get; set; }





    }
}
