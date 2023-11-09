using WebApi.Context;
using WebApi.Infrascture.Command;
using WebApi.Models;

namespace WebApi.Services.Contracts
{
    public interface IAuthenticationJWTService
    {
        AuthenticateResponse Authenticate(AuthenticateRequestCommand command);
        Task<int> CreateUserAsync(User user);
        List<User> GetAll();
        User GetById(int id);
    }
}
