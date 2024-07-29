using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using AMNApi.Entities;

namespace AMNApi.Data.Configurations;

public class ActiveUserAccountDoctorConfiguration : IEntityTypeConfiguration<ActiveUserAccountDoctor>
{
    public void Configure(EntityTypeBuilder<ActiveUserAccountDoctor> builder)
    {
        builder
                .HasNoKey()
                .ToView("VW_ActiveUserAccountDoctor");

        builder.Property(e => e.Email).HasMaxLength(150);
        builder.Property(e => e.FirstName).HasMaxLength(200);
        builder.Property(e => e.LastName).HasMaxLength(200);
        builder.Property(e => e.MiddleName).HasMaxLength(150);
        builder.Property(e => e.UserName).HasMaxLength(150);

        // Configure id don't delete
        builder.Property(e => e.Id).HasColumnName("UserAccountId");
    }
}