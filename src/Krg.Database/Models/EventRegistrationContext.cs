using Microsoft.EntityFrameworkCore;

namespace Krg.Database.Models
{
    public class EventRegistrationContext : DbContext
    {
        public EventRegistrationContext(DbContextOptions<EventRegistrationContext> options) : base(options)
        {
            
        }

        public DbSet<EventRegistration> Registrations { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{		
            modelBuilder.Entity<EventRegistration>().ToTable("Registration");
        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is EventRegistration && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((EventRegistration)entityEntry.Entity).UpdateTimeUtc = DateTime.UtcNow;
            }

            return base.SaveChanges();
        }
    }
}
