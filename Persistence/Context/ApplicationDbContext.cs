using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Persistence.Context;

public class ApplicationDbContext : IdentityDbContext<User>//, IApplicationDbContext
{
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<Good> Goods { get; set; } = null!;
    public DbSet<OrderedGood> OrderedGoods { get; set; } = null!;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        //Database.EnsureCreated();
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
        
        modelBuilder.Entity<User>().HasData(
            new User { UserName = "admin", /*Password = "111",*/ FirstName = "Иван", LastName = "Иванов", 
                PhoneNumber = "+79998887766", /*IsAdmin = true*/},
            new User { UserName = "user", /*Password = "222",*/ FirstName = "Пётр", LastName = "Петров", 
                PhoneNumber = "+79991119911", /*IsAdmin = false*/}
        );
        modelBuilder.Entity<Good>().HasData(
            new Good { Id = 1001, Name = "Товар 1", Price = 10 },
            new Good { Id = 1002, Name = "Товар 2", Price = 20 },
            new Good { Id = 1003, Name = "Товар 3", Price = 30 }
        );
    }
}

