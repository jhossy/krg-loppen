using NPoco;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Migrations;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Scoping;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Infrastructure.Migrations;
using Umbraco.Cms.Infrastructure.Migrations.Upgrade;
using Umbraco.Cms.Infrastructure.Persistence.DatabaseAnnotations;
using ColumnAttribute = NPoco.ColumnAttribute;

namespace Krg.Database
{
	public class RunEventRegistrationsMigration : INotificationHandler<UmbracoApplicationStartingNotification>
	{
		private readonly IMigrationPlanExecutor _migrationPlanExecutor;
		private readonly ICoreScopeProvider _coreScopeProvider;
		private readonly IKeyValueService _keyValueService;
		private readonly IRuntimeState _runtimeState;

		public RunEventRegistrationsMigration(
			ICoreScopeProvider coreScopeProvider,
			IMigrationPlanExecutor migrationPlanExecutor,
			IKeyValueService keyValueService,
			IRuntimeState runtimeState)
		{
			_migrationPlanExecutor = migrationPlanExecutor;
			_coreScopeProvider = coreScopeProvider;
			_keyValueService = keyValueService;
			_runtimeState = runtimeState;
		}

		public void Handle(UmbracoApplicationStartingNotification notification)
		{
			if (_runtimeState.Level < RuntimeLevel.Run)
			{
				return;
			}

			// Create a migration plan for a specific project/feature
			// We can then track that latest migration state/step for this project/feature
			var migrationPlan = new MigrationPlan("EventRegistration");

			// This is the steps we need to take
			// Each step in the migration adds a unique value
			migrationPlan.From(string.Empty)
				.To<AddEventRegistrationTable>("eventregistrations-db");

			// Go and upgrade our site (Will check if it needs to do the work or not)
			// Based on the current/latest step
			var upgrader = new Upgrader(migrationPlan);
			upgrader.Execute(
				_migrationPlanExecutor,
				_coreScopeProvider,
				_keyValueService);
		}
	}
	public class AddEventRegistrationTable : MigrationBase
	{
		public AddEventRegistrationTable(IMigrationContext context) : base(context)
		{
		}
		protected override void Migrate()
		{
			Logger.LogDebug("Running migration {MigrationStep}", "AddEventRegistrationTable");

			// Lots of methods available in the MigrationBase class - discover with this.
			if (TableExists("EventRegistration") == false)
			{
				Create.Table<EventRegistrationSchema>().Do();
			}
			else
			{
				Logger.LogDebug("The database table {DbTable} already exists, skipping", "EventRegistration");
			}
		}

		[TableName("EventRegistration")]
		[PrimaryKey("Id", AutoIncrement = true)]
		[ExplicitColumns]
		public class EventRegistrationSchema
		{
			[PrimaryKeyColumn(AutoIncrement = true, IdentitySeed = 1)]
			[Column("Id")]
			public int Id { get; set; }

			[Column("UmbracoEventNodeId")]
			public int UmbracoEventNodeId { get; set; }

			[Column("UpdateTimeUtc")]
			public DateTime UpdateTimeUtc { get; set; }

			[Column("EventDate")]
			public DateTime EventDate { get; set; }

			[Column("Name")]
			public string Name { get; set; } = null!;

			[Column("Department")]
			public string Department { get; set; } = null!;

			[Column("NoOfAdults")]
			public int NoOfAdults { get; set; }

			[Column("NoOfChildren")]
			public int NoOfChildren { get; set; }

			[Column("PhoneNo")]
			public string PhoneNo { get; set; } = null!;

			[Column("Email")]
			public required string Email { get; set; } = null!;

			[Column("BringsTrailer")]
			public bool BringsTrailer { get; set; }

			[Column("ShowName")]
			public bool ShowName { get; set; }
		}
	}
}
