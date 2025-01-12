using Krg.Database;
using Krg.Database.Extensions;
using Krg.Services.Extensions;
using Krg.Website.Extensions;
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
	builder.Services.AddEndpointsApiExplorer();
	builder.Services.AddSwaggerGen();

	builder.Services.AddSerilog((context, configuration) =>
	{
		Environment.SetEnvironmentVariable("BASEDIR", AppContext.BaseDirectory);

		configuration
			.ReadFrom.Configuration(builder.Configuration)
			.Enrich.FromLogContext();
	});

	builder.Services.AddDatabaseExtensions(builder.Configuration);
	builder.Services.AddServiceExtensions(builder.Configuration);
	builder.Services.AddWebsiteExtensions(builder.Configuration);
	builder.Services.AddScheduledJobs();
	
	var app = builder.Build();

	// Configure the HTTP request pipeline.
	if (!app.Environment.IsDevelopment())
	{
		app.UseExceptionHandler("/Home/Error");
		// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
		app.UseHsts();
	}
	else
	{
		app.UseSwagger();
		app.UseSwaggerUI();
	}

	using (var scope = app.Services.CreateScope())
	{
		var services = scope.ServiceProvider;

		var context = services.GetRequiredService<KrgContext>();

		
		//context.Database.EnsureCreated();
		context.Database.Migrate();		
// DbInitializer.Initialize(context);
	}
	
	using (var scope = app.Services.CreateScope())
	{
		var services = scope.ServiceProvider;

		var context = services.GetRequiredService<IdentityContext>();

		
		//context.Database.EnsureCreated();
		context.Database.Migrate();		
// DbInitializer.Initialize(context);
	}

	app.UseHttpsRedirection();
	app.UseStaticFiles();

	app.UseCookiePolicy(new CookiePolicyOptions
	{
		MinimumSameSitePolicy = SameSiteMode.Strict, Secure = CookieSecurePolicy.Always
	});

	app.UseRouting();

	app.UseAuthentication();
	app.UseAuthorization();

	app.MapControllerRoute(
		name: "Admin",
		pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
	);
	
	app.MapControllerRoute(
		name: "default",
		pattern: "{controller=Home}/{action=Index}/{id?}");

	app.MapIdentityApi<Microsoft.AspNetCore.Identity.IdentityUser>();
	
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