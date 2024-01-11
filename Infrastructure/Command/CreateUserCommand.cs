using MediatR;
using WebApi.Aplication.DTOs;

namespace WebApi.Infrastructure.Command
{
    public class CreateUserCommand : IRequest<int>
    {
        public string Username { get; set; }
        public string Password { get; set; }

    }
}
