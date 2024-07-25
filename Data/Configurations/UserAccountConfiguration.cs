using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using AMNApi.Entities;

namespace AMNApi.Data.Configurations;

public class UserAccountConfiguration : IEntityTypeConfiguration<UserAccount>
{
    public void Configure(EntityTypeBuilder<UserAccount> builder)
    {
        builder.Property(e => e.CreatedDate)
            .HasDefaultValueSql("(getdate())")
            .HasColumnType("datetime");
        builder.Property(e => e.Password).HasMaxLength(100);
        builder.Property(e => e.UserName).HasMaxLength(150);
        builder.Property(e => e.Email)
            .HasMaxLength(150)
            .IsUnicode(false);

        builder.HasMany(d => d.Doctor).WithMany(p => p.UserAccount)
            .UsingEntity<Dictionary<string, object>>(
                "UserAccountDoctor",
                r => r.HasOne<Doctor>().WithMany()
                    .HasForeignKey("DoctorId")
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserAccountDoctor_Doctor"),
                l => l.HasOne<UserAccount>().WithMany()
                    .HasForeignKey("UserAccountId")
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_UserAccountDoctor_UserAccount"),
                j =>
                {
                    j.HasKey("UserAccountId", "DoctorId");
                });

        builder.HasMany(d => d.Patient).WithMany(p => p.UserAccount)
            .UsingEntity<Dictionary<string, object>>(
                "UserAccountPatient",
                r => r.HasOne<Patient>().WithMany()
                    .HasForeignKey("PatientId")
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserAccountPatient_Patient"),
                l => l.HasOne<UserAccount>().WithMany()
                    .HasForeignKey("UserAccountId")
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_UserAccountPatient_UserAccount"),
                j =>
                {
                    j.HasKey("UserAccountId", "PatientId");
                });
    }
}