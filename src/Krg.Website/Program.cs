using Krg.Database;
using Krg.Database.Interfaces;
using Krg.Services;
using Krg.Services.Extensions;
using Krg.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
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

	builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
	builder.Services.AddTransient<IEventRegistrationService, EventRegistrationService>();
	builder.Services.AddSerilog((context, configuration) =>
	{
		Environment.SetEnvironmentVariable("BASEDIR", AppContext.BaseDirectory);

		configuration
			.ReadFrom.Configuration(builder.Configuration)
			.Enrich.FromLogContext();
	});

	builder.Services.AddDbContext<KrgContext>(options =>
		options.UseSqlServer(builder.Configuration.GetConnectionString("KrgContext")));

	builder.Services.AddServiceExtensions(builder.Configuration);

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