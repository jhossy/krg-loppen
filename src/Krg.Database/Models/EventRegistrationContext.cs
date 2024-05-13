using Microsoft.EntityFrameworkCore;

namespace Krg.Database.Models
{
    public class EventRegistrationContext : DbContext
    {
        public EventRegistrationContext(DbContextOptions<EventRegistrationContext> options) : base(options)
        {
            
        }

        public DbSet<EventRegistration> Registrations { get; set; }

		public DbSet<EmailNotification> Notifications { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{		
            modelBuilder.Entity<EventRegistration>().ToTable("Registration");
			modelBuilder.Entity<EmailNotification>().ToTable("Notification");
		}

        public override int SaveChanges()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is IUpdateableEntity && 
                            (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((IUpdateableEntity)entityEntry.Entity).UpdateTimeUtc = DateTime.UtcNow;
            }

            return base.SaveChanges();
        }
    }
}
