using System.Security.Claims;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Context;

public class ApplicationDbContext : IdentityDbContext<User>, IApplicationDbContext
{
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<Good> Goods { get; set; } = null!;
    public DbSet<OrderedGood> OrderedGoods { get; set; } = null!;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Order>()
            .HasKey(order => order.Id); 
        modelBuilder.Entity<OrderedGood>()
            .HasKey(orderedGood => new { orderedGood.OrderId, orderedGood.GoodId });
        modelBuilder.Entity<Good>()
            .HasKey(good => good.Id); 
        
    
        var hasher = new PasswordHasher<User>();
        modelBuilder.Entity<User>().HasData(
            new User 
            {
                Id = "admin",
                UserName = "admin",
                PasswordHash = hasher.HashPassword(null!, "admin"),
                
                FirstName = "Иван",
                LastName = "Иванов",
                NormalizedUserName = "admin".ToUpper(),
                Email = "admin@mail.ru",
                NormalizedEmail = "admin@mail.ru".ToUpper(),
                EmailConfirmed = true
            });
        
        modelBuilder.Entity<IdentityUserClaim<string>>().HasData(
            new IdentityUserClaim<string>
            {
                Id = 1,
                UserId = "admin",
                ClaimType = ClaimTypes.Role,
                ClaimValue = "user"
            },
            new IdentityUserClaim<string>
            {
                Id = 2,
                UserId = "admin",
                ClaimType = ClaimTypes.Role,
                ClaimValue = "admin"
            },
            new IdentityUserClaim<string>
            {
                Id = 3,
                UserId = "admin",
                ClaimType = ClaimTypes.Name,
                ClaimValue = "admin"
            }
        );
        
        modelBuilder.Entity<Good>().HasData(
            new Good { Id = 1001, Name = "Товар 1", Price = 10 },
            new Good { Id = 1002, Name = "Товар 2", Price = 20 },
            new Good { Id = 1003, Name = "Товар 3", Price = 30 }
        );
    }
}

