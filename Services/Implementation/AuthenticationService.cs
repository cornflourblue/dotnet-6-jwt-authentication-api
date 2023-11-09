namespace WebApi.Services.Implementation;

using FluentValidation;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApi.Context;
using WebApi.Helpers;
using WebApi.Infrascture.Command;
using WebApi.Models;
using WebApi.Services.Contracts;

public class AuthenticationService : IAuthenticationJWTService
{
    private readonly tokenjwtContext context;
    private readonly AppSettings appSettings;
    private readonly IValidator<User> validator;

    public AuthenticationService(tokenjwtContext context,
                                 IOptions<AppSettings> appSettings,
                                 IValidator<User> validator)
    {
        this.context = context;
        this.appSettings = appSettings.Value;
        this.validator = validator;
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
        return this.context.Users.FirstOrDefault(x => x.Iduser == id);
    }

    public async Task<int> CreateUserAsync(User user)
    {
        using var context = this.context;
        this.context.Users.Add(user);
        await this.context.SaveChangesAsync();
        return user.Iduser;
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