using FluentValidation;
using System.Reflection;
using WebApi.Aplication.Handlers.Mapping;
using WebApi.Aplication.Handlers.Validator;

namespace WebApi.Aplication.IoC
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddCustomApplicationServices(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            return services;
        }
    }
}
