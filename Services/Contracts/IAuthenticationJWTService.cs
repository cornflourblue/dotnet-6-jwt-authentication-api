using WebApi.Entities;
using WebApi.Infrascture.Command;
using WebApi.Models;

namespace WebApi.Services.Contracts
{
    public interface IAuthenticationJWTService
    {
        AuthenticateResponse Authenticate(AuthenticateRequestCommand command);
        List<User> GetAll();
        User GetById(int id);
    }
}
