using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using LoggerService;

namespace CompanyEmployees.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureLoggerService(this IServiceCollection services) =>
            services.AddSingleton<ILoggerManager, LoggerManager>();

        public static void ConfigureIISIntegration(this IServiceCollection services) =>
            services.Configure<IISOptions>(options => { });

        public static void ConfigureCors(this IServiceCollection services) =>
            services.AddCors(options =>
            {
                options.AddPolicy(
                    "CorsPolicy",
                    builder =>
                        builder
                            .AllowAnyOrigin() //  WithOrigins("https://example.com")
                            .AllowAnyMethod() // WithMethods("POST", "GET")
                            .AllowAnyHeader()
                ); // WithHeaders("accept", "content-type")
            });
    }
}


/*
By calling the services.AddSingleton method, we can create a
service the first time we request it and then every subsequent
request will call the same instance of the service. This means that all
components share the same service every time they need it and the
same instance will be used for every method call.
• By calling the services.AddScoped method, we can create
a service once per request. That means whenever we send an HTTP
request to the application, a new instance of the service will be
created.
• By calling the services.AddTransient method, we can create a
service each time the application requests it. This means that if
multiple components need the service, it will be created again for
every single component request.
*/
