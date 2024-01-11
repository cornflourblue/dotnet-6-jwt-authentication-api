using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Configuration;
using WebApi.Context;
using WebApi.Middkeware;
using WebApi.Models.Config;
using WebApi.Models.RabbitMQ;
using WebApi.Services.Contracts;
using WebApi.Services.Contracts.Implementation;

var builder = WebApplication.CreateBuilder(args);
{
    var services = builder.Services;
    var azureAppConfigConnectionString = builder.Configuration["ConnectionStrings:AppConfig"];
    var configBuilder = new ConfigurationBuilder();
    configBuilder.AddAzureAppConfiguration(options =>
    {
        options.Connect(azureAppConfigConnectionString)
               .UseFeatureFlags();
    });

    var azureAppConfig = configBuilder.Build();
    services.AddSingleton<IConfiguration>(azureAppConfig);
    services.AddCors();
    services.AddControllers();
    services.AddMediatR(typeof(Program).Assembly);
    services.AddCustomApplicationServices();
    services.AddScoped<IAuthenticationJWTService, AuthenticationService>();
    services.AddDbContext<tokenjwtContext>(opt =>
    {
        opt.UseSqlServer(azureAppConfig["MicroserviceAuthentication:Server"]);
    });
    
}

var app = builder.Build();
{
    app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
  
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        c.RoutePrefix = "swagger";
    });
    app.UseMiddleware<JwtMiddleware>();
    app.MapControllers();
}

app.Run();