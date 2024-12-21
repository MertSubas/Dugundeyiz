using Dugundeyiz.Entity;
using Dugundeyiz.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Dugundeyiz.Context
{
    public class DugundeyizContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public DugundeyizContext(DbContextOptions<DugundeyizContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<City> Citys { get; set; }
        public DbSet<Town> Towns { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<UserPartner> UserPartners { get; set; }
        public DbSet<ProductPhoto> ProductPhotos { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<UserApproval> UserApprovals { get; set; }




    }
}
