using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using AMNApi.Entities;

namespace AMNApi.Data.Configurations;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.HasOne(d => d.Consultory).WithMany(p => p.Appointment)
            .HasForeignKey(d => d.ConsultoryId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Appointment_Consultory");

        builder.HasOne(d => d.Doctor).WithMany(p => p.Appointment)
            .HasForeignKey(d => d.DoctorId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Appointment_Doctor");

        builder.HasOne(d => d.Patient).WithMany(p => p.Appointment)
            .HasForeignKey(d => d.PatientId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Appointment_Patient");
    }
}