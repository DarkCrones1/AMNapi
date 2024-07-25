using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using AMNApi.Entities;

namespace AMNApi.Data.Configurations;

public class PatientAdressConfiguration : IEntityTypeConfiguration<PatientAddress>
{
    public void Configure(EntityTypeBuilder<PatientAddress> builder)
    {
        builder.HasKey(e => e.Id).HasName("PK__PatientAddress__3214EC07A20A3AD6");

        builder.Property(e => e.RegisterDate)
            .HasDefaultValueSql("(getdate())")
            .HasColumnType("datetime");

        builder.HasOne(d => d.Address).WithMany(p => p.PatientAddress)
            .HasForeignKey(d => d.AddressId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_PatientAddress_Address");

        builder.HasOne(d => d.Patient).WithMany(p => p.PatientAddress)
            .HasForeignKey(d => d.PatientId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_PatientAddress_Patient");
    }
}