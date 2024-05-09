﻿using Krg.Database;
using Krg.Services.Interfaces;
using Krg.Web.Controllers;
using Microsoft.Extensions.DependencyInjection;

namespace Krg.Services.Extensions
{
    public static class ServicesExtensions
	{
		public static IServiceCollection AddServiceExtensions(this IServiceCollection services)
		{
			//services.AddTransient<IUnitOfWork, UnitOfWork>();
			//services.AddTransient<IEventRegistrationRepository, EventRegistrationRepository>();
			services.AddTransient<IEventRegistrationService, EventRegistrationService>();
			services.AddTransient<IRegistrationRepository, RegistrationRepository>();
			services.AddTransient<IExcelService, ExcelService>();

			return services;
		}
	}
}
