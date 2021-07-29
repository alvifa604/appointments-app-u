using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            //Configura la tabla de muchos a muchos
            builder.HasKey(e => new { e.Id });
            builder.HasOne(p => p.Patient).WithMany(a => a.Appointments).HasForeignKey(p => p.PatientId);
            builder.HasOne(s => s.Service).WithMany(a => a.Appointments).HasForeignKey(s => s.ServiceId);
        }
    }
}