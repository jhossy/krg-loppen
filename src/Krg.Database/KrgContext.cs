﻿using Krg.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Krg.Database
{
	public class KrgContext : DbContext
	{
		public KrgContext(DbContextOptions<KrgContext> options)
		   : base(options)
		{
		}

		public DbSet<EmailNotification> EmailNotifications { get; set; }

		public DbSet<EmailReminderNotification> EmailReminderNotifications { get; set; }

		public DbSet<EventRegistration> EventRegistrations { get; set; }
		
		public DbSet<Event> Events { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<EmailNotification>().ToTable("EmailNotification");
			modelBuilder.Entity<EmailReminderNotification>().ToTable("EmailReminderNotification");
			modelBuilder.Entity<EventRegistration>().ToTable("EventRegistration");
			modelBuilder.Entity<Event>().ToTable("Event");
		}

		public override int SaveChanges()
		{
			foreach (var entry in ChangeTracker.Entries())
			{
				switch (entry.State)
				{
					case EntityState.Modified:
						((IUpdateableEntity)entry.Entity).UpdateTimeUtc = DateTime.UtcNow;
						break;
				}
			}
			return base.SaveChanges();
		}
	}
}
