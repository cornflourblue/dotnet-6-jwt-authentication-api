namespace WebApi.Models;

using WebApi.Entities;

public class AuthenticateResponse
{
    public string Token { get; set; }

    public AuthenticateResponse( string token)
    {
        Token = token;
    }
}