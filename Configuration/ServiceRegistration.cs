using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace WebApi.Configuration
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddCustomApplicationServices(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
 
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Token JWT", Version = "v1" });
            });
            services.AddSwaggerGen();
            return services;
        }
    }
}
