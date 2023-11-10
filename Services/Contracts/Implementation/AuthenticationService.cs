namespace WebApi.Services.Contracts.Implementation;

using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApi.Context;
using WebApi.Infrascture.Command;
using WebApi.Models;
using WebApi.Models.Config;
using WebApi.Models.RabbitMQ;
using WebApi.Services.Contracts;

public class AuthenticationService : IAuthenticationJWTService
{
    private readonly tokenjwtContext context;
    private readonly AppSettings appSettings;
    private readonly IValidator<User> validator;
    private readonly IConfiguration configuration;

    public AuthenticationService(tokenjwtContext context,
                                 IOptions<AppSettings> appSettings,
                                 IValidator<User> validator,
                                 IConfiguration configuration)
    {
        this.context = context;
        this.appSettings = appSettings.Value;
        this.validator = validator;
        this.configuration = configuration;
    }


    public AuthenticateResponse Authenticate(AuthenticateRequestCommand command)
    {
        using var context = this.context;
        var user = this.context.Users.FirstOrDefault(u => u.Name == command.Username && u.Password == command.Password);
        return user is not null ? new AuthenticateResponse(GenerateJwtToken(user)) : null;
    }

    public List<User> GetAll()
    {
        using (var context = this.context)
            return this.context.Users.ToList();
    }

    public User GetById(int id)
    {
        return context.Users.FirstOrDefault(x => x.Iduser == id);
    }

    public async Task<int> CreateUserAsync(User user)
    {
        using var context = this.context;
        this.context.Users.Add(user);
        await this.context.SaveChangesAsync();
        //var SendSMS = new SendSMSService();
        //await SendSMS.SendAsync();
        foreach (var key in configuration.AsEnumerable())
        {
            Console.WriteLine($"Key: {key.Key}, Value: {key.Value}");
        }

        this.NotificationEventBus();
        return user.Iduser;
    }

    private RabbitMQConfiguration retrive(IConfiguration configuration)
    {
        return new RabbitMQConfiguration()
        {
            Uri = configuration["MicroserviceAuthentication:RabbitMQ:Uri"].Trim(),
            Queue = configuration["MicroserviceAuthentication:RabbitMQ:Queue"].Trim(),
            durable = bool.TryParse(configuration["MicroserviceAuthentication:RabbitMQ:durable"], out bool durable) && durable,
            exclusive = bool.TryParse(configuration["MicroserviceAuthentication:RabbitMQ:exclusive"], out bool exclusive) && exclusive,
            autoDelete = bool.TryParse(configuration["MicroserviceAuthentication:RabbitMQ:autoDelete"], out bool autoDelete) && autoDelete,
            arguments = null ,//JsonConvert.DeserializeObject<Dictionary<string, object>>(configuration["MicroserviceAuthentication:RabbitMQ:arguments"]) ?? null,
            exchange = configuration["MicroserviceAuthentication:RabbitMQ:exchange"].Trim(),
        };
    }

    private void NotificationEventBus()
    {
        var rabbitMQConfigurationRetrive = this.retrive(configuration);
        var factory = new ConnectionFactory() { Uri = new Uri(rabbitMQConfigurationRetrive.Uri) };
        using (var connection = factory.CreateConnection())
        {
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: rabbitMQConfigurationRetrive.Queue,
                                     durable: rabbitMQConfigurationRetrive.durable,
                                     exclusive: rabbitMQConfigurationRetrive.exclusive,
                                     autoDelete: rabbitMQConfigurationRetrive.autoDelete,
                                     arguments: rabbitMQConfigurationRetrive.arguments);

                string jsonMessage = JsonConvert.SerializeObject(rabbitMQConfigurationRetrive);
                byte[] body = Encoding.UTF8.GetBytes(jsonMessage);
                channel.BasicPublish(exchange: rabbitMQConfigurationRetrive.exchange,
                                     routingKey: rabbitMQConfigurationRetrive.Queue,
                                     basicProperties: null,
                                     body: body);
            }
        }
    }

    private string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(appSettings.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.IsPersistent, user.Iduser.ToString()),
                new Claim(ClaimTypes.Name, user.Name.ToString())
            }
            ),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}