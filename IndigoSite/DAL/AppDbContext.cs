using IndigoSite.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IndigoSite.DAL
{
    public class AppDbContext: IdentityDbContext
    {
        public AppDbContext( DbContextOptions<AppDbContext> options): base(options) { }
        public DbSet<Post> Posts { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
    }
}
