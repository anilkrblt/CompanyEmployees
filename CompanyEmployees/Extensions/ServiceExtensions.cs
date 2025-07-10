using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyEmployees.Extensions
{
    public static class ServiceExtensions
    {
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
