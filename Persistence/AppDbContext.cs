using System.Reflection;
using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    /// <summary>
    /// Se encarga del manejo de la base de datos
    /// Se utiliza identity para el manejo de usuarios
    /// </summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }
        public DbSet<User> User { get; set; }
        public DbSet<Appointment> Appointment { get; set; }
        public DbSet<Service> Service { get; set; }
        public DbSet<Role> Role { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Obtiene las configuraciones que se le hizo a la tabla de pacientes
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Entity<Role>()
                .HasIndex(p => p.Name);
        }
    }
}