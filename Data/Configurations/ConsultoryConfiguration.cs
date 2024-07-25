using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using AMNApi.Entities;

namespace AMNApi.Data.Configurations;

public class ConsultoryConfiguration : IEntityTypeConfiguration<Consultory>
{
    public void Configure(EntityTypeBuilder<Consultory> builder)
    {

        builder.Property(e => e.Name)
            .HasMaxLength(200)
            .IsUnicode(false);
        builder.Property(e => e.Phone)
            .HasMaxLength(50)
            .IsUnicode(false);

        builder.HasMany(d => d.Address).WithMany(p => p.Consultory)
            .UsingEntity<Dictionary<string, object>>(
                "ConsultoryAddress",
                r => r.HasOne<Address>().WithMany()
                    .HasForeignKey("AddressId")
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ConsultoryAddress_Address"),
                l => l.HasOne<Consultory>().WithMany()
                    .HasForeignKey("ConsultoryId")
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ConsultoryAddress_Consultory"),
                j =>
                {
                    j.HasKey("ConsultoryId", "AddressId");
                });
    }
}