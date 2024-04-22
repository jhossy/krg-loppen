using Krg.Database.Models;
using Krg.Services.Extensions;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.CreateUmbracoBuilder()
    .AddBackOffice()
    .AddWebsite()
    .AddDeliveryApi()
    .AddComposers()
    .Build();

//builder.Services.AddDbContext<EventRegistrationContext>(options =>  options.UseSqlServer(builder.Configuration.GetConnectionString("EventRegistrationContext")));

builder.Services.AddServiceExtensions();

WebApplication app = builder.Build();

app.UseStaticFiles();

await app.BootUmbracoAsync();

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

//using (var scope = app.Services.CreateScope())
//{
//	var services = scope.ServiceProvider;

//	var context = services.GetRequiredService<EventRegistrationContext>();
//	context.Database.EnsureCreated();
//}

await app.RunAsync();
