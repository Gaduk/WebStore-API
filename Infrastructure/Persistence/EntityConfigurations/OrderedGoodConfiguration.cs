using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityConfigurations;

public class OrderedGoodConfiguration: IEntityTypeConfiguration<OrderedGood>
{
    public void Configure(EntityTypeBuilder<OrderedGood> builder)
    {
        builder.HasKey(orderedGood => orderedGood.Id);
        builder
            .HasOne(og => og.Good)
            .WithMany()
            .HasForeignKey(og => og.GoodId);
    }
}