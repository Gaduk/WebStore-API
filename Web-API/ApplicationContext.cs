namespace Web_API;

using Microsoft.EntityFrameworkCore;
public class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<Good> Goods { get; set; } = null!;
    public DbSet<OrderedGood> OrderedGoods { get; set; } = null!;
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
        //Database.EnsureDeleted();
        Database.EnsureCreated();
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasKey(user => user.Login); 
        modelBuilder.Entity<Order>()
            .HasKey(order => order.ID); 
        modelBuilder.Entity<OrderedGood>()
            .HasKey(orderedGood => new { orderedGood.OrderID, orderedGood.GoodID });
        modelBuilder.Entity<Good>()
            .HasKey(good => good.ID); 
        
        modelBuilder.Entity<User>().HasData(
            new User { Login = "admin", Password = "111", FirstName = "Иван", LastName = "Иванов", 
                PhoneNumber = "+79998887766", IsAdmin = true},
            new User { Login = "user", Password = "222", FirstName = "Пётр", LastName = "Петров", 
                PhoneNumber = "+79991119911", IsAdmin = false},
            new User { Login = "user2", Password = "333", FirstName = "Семён", LastName = "Семёнов", 
                PhoneNumber = "+79991119922", IsAdmin = false}
        );
        modelBuilder.Entity<Good>().HasData(
            new Good { ID = 1001, Name = "Товар 1", Price = 10 },
            new Good { ID = 1002, Name = "Товар 2", Price = 20 },
            new Good { ID = 1003, Name = "Товар 3", Price = 30 }
        );
    }
}

