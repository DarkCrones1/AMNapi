using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using AMNApi.Entities;

namespace AMNApi.Data.Configurations;

public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        // Don't delete auxiliar property
        builder.Ignore(x => x.FullAddress);

        builder.Property(e => e.Address1).HasMaxLength(150);
        builder.Property(e => e.Address2).HasMaxLength(150);
        builder.Property(e => e.ExternalNumber).HasMaxLength(10);
        builder.Property(e => e.InternalNumber).HasMaxLength(10);
        builder.Property(e => e.Street).HasMaxLength(50);
        builder.Property(e => e.ZipCode).HasMaxLength(10);
    }
}