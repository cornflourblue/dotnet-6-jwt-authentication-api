using MediatR;
using WebApi.Aplication.IoC;
using WebApi.Helpers;
using WebApi.Services.Contracts;
using WebApi.Services.Implementation;

var builder = WebApplication.CreateBuilder(args);
{
    var services = builder.Services;
    services.AddCors();
    services.AddControllers();
    services.AddMediatR(typeof(Program).Assembly);
    services.AddCustomApplicationServices();
    services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
    services.AddScoped<IAuthenticationJWTService, AuthenticationService>();
    
}

var app = builder.Build();

// configure HTTP request pipeline
{
    // global cors policy
    app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

    // custom jwt auth middleware
    app.UseMiddleware<JwtMiddleware>();

    app.MapControllers();
}

app.Run("http://localhost:4000");