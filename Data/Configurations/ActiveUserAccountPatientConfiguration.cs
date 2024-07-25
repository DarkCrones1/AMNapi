using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using AMNApi.Entities;

namespace AMNApi.Data.Configurations;

public class ActiveUserAccountPatientConfiguration : IEntityTypeConfiguration<ActiveUserAccountPatient>
{
    public void Configure(EntityTypeBuilder<ActiveUserAccountPatient> builder)
    {
        builder
                .HasNoKey()
                .ToView("VW_ActiveUserAccountPatient");

        builder.Property(e => e.CellPhone).HasMaxLength(20);
        builder.Property(e => e.Email).HasMaxLength(150);
        builder.Property(e => e.FirstName).HasMaxLength(200);
        builder.Property(e => e.LastName).HasMaxLength(200);
        builder.Property(e => e.MiddleName).HasMaxLength(150);
        builder.Property(e => e.UserName).HasMaxLength(150);

        // Configure id don't delete
        builder.Property(e => e.Id).HasColumnName("UserAccountId");
    }
}