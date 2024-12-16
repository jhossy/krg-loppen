using Krg.Database;
using Krg.Database.Extensions;
using Krg.Database.Interfaces;
using Krg.Services;
using Krg.Services.Extensions;
using Krg.Services.Interfaces;
using Krg.Website.Extensions;
using Krg.Website.Jobs;
using Microsoft.EntityFrameworkCore;
using Quartz;
using Serilog;

Log.Logger = new LoggerConfiguration()
	.WriteTo.Console()
	.CreateBootstrapLogger();

try
{
	Log.Information("Starting web application");
	var builder = WebApplication.CreateBuilder(args);

	// Add services to the container.
	builder.Services.AddControllersWithViews();
		
	builder.Services.AddSerilog((context, configuration) =>
	{
		Environment.SetEnvironmentVariable("BASEDIR", AppContext.BaseDirectory);

		configuration
			.ReadFrom.Configuration(builder.Configuration)
			.Enrich.FromLogContext();
	});

	builder.Services.AddDbContext<KrgContext>(options =>
		options.UseSqlServer(builder.Configuration.GetConnectionString("KrgContext")));

	builder.Services.AddDatabaseExtensions();
	builder.Services.AddServiceExtensions(builder.Configuration);
	builder.Services.AddWebsiteExtensions(builder.Configuration);

	//quartz
	builder.Services.AddQuartz(q =>
	{
		var jobKey = new JobKey(nameof(ConsoleJob));
			

		q.AddJob<ConsoleJob>(jobKey)						
			.AddTrigger(opts => opts.ForJob(jobKey)
					.WithIdentity("ConsoleJob-trigger")
					.WithCronSchedule("0 * * ? * *"))
			.UsePersistentStore(s =>
			{
				
				s.RetryInterval = TimeSpan.FromSeconds(1);
				s.UseMicrosoftSQLite(
					options =>
					{
						options.ConnectionStringName = "Jobs";
						//options.ConnectionString = "Data Source=C:\\Development\\krg-loppen\\src\\Krg.Infrastructure\\jobs.db";
						//options.ConnectionString = "Data Source=jobs.db";
					}
				);
				s.UseNewtonsoftJsonSerializer();
				//s.UseSqlServer(sqlserver =>
				//{
				//	sqlserver.ConnectionString = "Server=.\\sqlexpress;Database=quartz-sample;Integrated Security=True;TrustServerCertificate=True;MultipleActiveResultSets=true";
				//});
			});
		
	});
	//quartz end

	builder.Services.AddQuartzHostedService(opt =>
	{
		opt.WaitForJobsToComplete = true;
	});

	var app = builder.Build();

	// Configure the HTTP request pipeline.
	if (!app.Environment.IsDevelopment())
	{
		app.UseExceptionHandler("/Home/Error");
		// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
		app.UseHsts();
	}

	using (var scope = app.Services.CreateScope())
	{
		var services = scope.ServiceProvider;

		var context = services.GetRequiredService<KrgContext>();

		context.Database.EnsureCreated();
		// DbInitializer.Initialize(context);
	}

	app.UseHttpsRedirection();
	app.UseStaticFiles();

	app.UseRouting();

	app.UseAuthorization();

	app.MapControllerRoute(
		name: "default",
		pattern: "{controller=Home}/{action=Index}/{id?}");

	app.Run();

	Log.Information("Stopped cleanly");
	return 0;
}
catch (Exception ex)
{
	Log.Fatal(ex, "Application terminated unexpectedly");
	return 1;
}
finally
{
	Log.CloseAndFlush();
}