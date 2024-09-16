using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityConfigurations;

public class GoodConfiguration: IEntityTypeConfiguration<Good>
{
    public void Configure(EntityTypeBuilder<Good> builder)
    {
        builder.HasKey(good => good.Id); 
        
        builder.HasData(
            new Good { Id = 1001, Name = "Товар 1", Price = 10 },
            new Good { Id = 1002, Name = "Товар 2", Price = 20 },
            new Good { Id = 1003, Name = "Товар 3", Price = 30 }
        );
    }
}