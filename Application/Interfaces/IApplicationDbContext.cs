using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Application.Interfaces;

public interface IApplicationDbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Good> Goods { get; set; }
    public DbSet<OrderedGood> OrderedGoods { get; set; }
    //Task<int> SaveChanges();
}