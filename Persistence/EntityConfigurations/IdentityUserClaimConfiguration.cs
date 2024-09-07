using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

public class IdentityUserClaimConfiguration: IEntityTypeConfiguration<IdentityUserClaim<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserClaim<string>> builder)
    {
        builder.HasData(
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
    }
}