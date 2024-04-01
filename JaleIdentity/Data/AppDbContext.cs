using JaleIdentity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JaleIdentity.Data;

public class AppDbContext:IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> opt):base(opt)
    {
        
    }


    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Product> Products  { get; set; } = null!;
}
