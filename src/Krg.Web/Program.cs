using Krg.Services.Extensions;
using Krg.Web.Extensions;
using Microsoft.AspNetCore.Rewrite;
using Serilog;

Log.Logger = new LoggerConfiguration()
	.WriteTo.Console()
	.CreateBootstrapLogger();

try
{
	Log.Information("Starting web application");

	WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

	builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);
    
	builder.CreateUmbracoBuilder()
        .AddBackOffice()
        .AddWebsite()
        .AddDeliveryApi()
        .AddComposers()
        .Build();

    builder.Services.AddSerilog((context, configuration) =>
    {
		Environment.SetEnvironmentVariable("BASEDIR", AppContext.BaseDirectory);

		configuration
			.ReadFrom.Configuration(builder.Configuration)
			.Enrich.FromLogContext();
    });

	builder.Services.AddServiceExtensions(builder.Configuration);
    
	WebApplication app = builder.Build();

    app.UseStaticFiles();

    await app.BootUmbracoAsync();

	if (!builder.Environment.IsAzure())
	{
		app.UseHttpsRedirection();
		app.UseRewriter(new RewriteOptions().AddRedirectToWwwPermanent());
	}

	app.UseUmbraco()
        .WithMiddleware(u =>
        {
            u.UseBackOffice();
            u.UseWebsite();
        })
        .WithEndpoints(u =>
        {
            u.UseInstallerEndpoints();
            u.UseBackOfficeEndpoints();
            u.UseWebsiteEndpoints();
        });

	// Write streamlined request completion events, instead of the more verbose ones from the framework.
	// To use the default framework request logging instead, remove this line and set the "Microsoft"
	// level in appsettings.json to "Information".
	app.UseSerilogRequestLogging();

	//using (var scope = app.Services.CreateScope())
	//{
	//    var services = scope.ServiceProvider;

	//    var context = services.GetRequiredService<EventRegistrationContext>();
	//    context.Database.EnsureCreated();
	//}

	await app.RunAsync();

	Log.Information("Stopped cleanly");
	return 0;
}
catch(Exception ex)
{
	Log.Fatal(ex, "Application terminated unexpectedly");
	return 1;
}
finally
{
	Log.CloseAndFlush();
}