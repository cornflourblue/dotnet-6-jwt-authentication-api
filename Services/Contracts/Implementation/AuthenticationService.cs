namespace WebApi.Services.Contracts.Implementation;

using FluentValidation;
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
using WebApi.Helpers;
using WebApi.Infrascture.Command;
using WebApi.Models;
using WebApi.Models.RabbitMQ;
using WebApi.Services.Contracts;
using WebApi.Services.SMS;

public class AuthenticationService : IAuthenticationJWTService
{
    private readonly tokenjwtContext context;
    private readonly AppSettings appSettings;
    private readonly IValidator<User> validator;
    private readonly RabbitMQConfiguration rabbitMQConfiguration;

    public AuthenticationService(tokenjwtContext context,
                                 IOptions<AppSettings> appSettings,
                                 IValidator<User> validator,
                                 IOptions<RabbitMQConfiguration> rabbitMQConfiguration)
    {
        this.context = context;
        this.appSettings = appSettings.Value;
        this.validator = validator;
        this.rabbitMQConfiguration = rabbitMQConfiguration.Value;
    }


    public AuthenticateResponse Authenticate(AuthenticateRequestCommand command)
    {
        using var context = this.context;
        var user = this.context.Users.FirstOrDefault(u => u.Name == command.Username && u.Password == command.Password);
        return user is not null ? new AuthenticateResponse(generateJwtToken(user)) : null;
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
        var SendSMS = new SendSMSService();
        await SendSMS.SendAsync();
        this.NotificationEventBus();
        return user.Iduser;
    }


    private void NotificationEventBus()
    {
        Dictionary<string, object> argumentsConfig = this.rabbitMQConfiguration.arguments;
        var factory = new ConnectionFactory() { Uri = new Uri(this.rabbitMQConfiguration.Uri) };
        using (var connection = factory.CreateConnection())
        {
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: this.rabbitMQConfiguration.Queue,
                                     durable: this.rabbitMQConfiguration.durable,
                                     exclusive: this.rabbitMQConfiguration.exclusive,
                                     autoDelete: this.rabbitMQConfiguration.autoDelete,
                                     arguments: argumentsConfig);

                string jsonMessage = JsonConvert.SerializeObject(this.rabbitMQConfiguration);
                byte[] body = Encoding.UTF8.GetBytes(jsonMessage);
                channel.BasicPublish(exchange: this.rabbitMQConfiguration.exchange,
                                     routingKey: this.rabbitMQConfiguration.Queue,
                                     basicProperties: this.rabbitMQConfiguration.basicProperties,
                                     body: body);
            }
        }
    }

    private string generateJwtToken(User user)
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