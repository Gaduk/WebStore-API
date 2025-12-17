using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityConfigurations;

public class BasketItemConfiguration : IEntityTypeConfiguration<BasketItem>
{
    public void Configure(EntityTypeBuilder<BasketItem> builder)
    {
        
        builder.HasKey(basketItem => new 
        { 
            basketItem.UserName, 
            basketItem.GoodId 
        });
        
        builder
            .HasOne(basketItem => basketItem.Good)
            .WithMany()
            .HasForeignKey(basketItem => basketItem.GoodId);
    }
}