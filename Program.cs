using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebApi.Configuration;
using WebApi.Context;
using WebApi.Helpers;
using WebApi.Models.RabbitMQ;
using WebApi.Services.Contracts;
using WebApi.Services.Contracts.Implementation;

var builder = WebApplication.CreateBuilder(args);
{
    var services = builder.Services;
    services.AddCors();
    services.AddControllers();
    services.AddMediatR(typeof(Program).Assembly);
    services.AddCustomApplicationServices();
    services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
    services.Configure<RabbitMQConfiguration>(builder.Configuration.GetSection("RabbitMQ"));
    services.AddScoped<IAuthenticationJWTService, AuthenticationService>();
    services.AddDbContext<tokenjwtContext>(opt =>
    {
        opt.UseSqlServer(builder.Configuration.GetConnectionString("Server"));
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