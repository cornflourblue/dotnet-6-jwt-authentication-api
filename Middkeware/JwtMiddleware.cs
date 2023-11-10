namespace WebApi.Middkeware;

using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.Models.Config;
using WebApi.Services.Contracts;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration configuration;

    public JwtMiddleware(RequestDelegate next,
                         IConfiguration configuration)
    {
        _next = next;
        this.configuration = configuration;
    }

    public async Task Invoke(HttpContext context, IAuthenticationJWTService userService)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token != null)
            attachUserToContext(context, userService, token);

        await _next(context);
    }

    private AppSettings retrive(IConfiguration configuration)
    {
        return new AppSettings()
        {
            Secret = configuration["MicroserviceAuthentication:AppSettings:Secret"].Trim(),
          
        };
    }

    private void attachUserToContext(HttpContext context, IAuthenticationJWTService userService, string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(this.retrive(this.configuration).Secret);
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = int.Parse(jwtToken.Claims.First(x => x.Type == ClaimTypes.IsPersistent).Value);
            context.Items["User"] = userService.GetById(userId);
        }
        catch {}
    }
}