using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using AMNApi.Entities;

namespace AMNApi.Data.Configurations;

public class MapLocationConfiguration : IEntityTypeConfiguration<MapLocation>
{
    public void Configure(EntityTypeBuilder<MapLocation> builder)
    {
        builder.HasOne(d => d.Consultory)
            .WithMany(p => p.MapLocation)
            .HasForeignKey(d => d.ConsultoryId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_MapLocation_Consultory");
    }
}