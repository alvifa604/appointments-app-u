using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class UserConfigurations : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(p => p.IdDocument).IsRequired().HasMaxLength(12); 
            builder.Property(p => p.Name).IsRequired().HasMaxLength(50);
            builder.Property(p => p.FirstLastname).IsRequired().HasMaxLength(30);
            builder.Property(p => p.SecondLastname).IsRequired().HasMaxLength(30);
            builder.Property(p => p.Email).IsRequired().HasMaxLength(50);
            builder.Property(p => p.Password).IsRequired();
            builder.Property(p => p.PhoneNumber).IsRequired().HasMaxLength(8);

            builder.HasIndex(x => x.IdDocument);
        }
    }
}