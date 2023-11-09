namespace WebApi.Aplication.Handlers.Command
{
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;
    using WebApi.Context;
    using WebApi.Infrastructure.Command;
    using WebApi.Services.Contracts;

    public class CreateUserHandler : IRequestHandler<CreateUserCommand, int>
    {
        private readonly IAuthenticationJWTService authentacationService;

        public CreateUserHandler(IAuthenticationJWTService authentacationService)
        {
            this.authentacationService = authentacationService;
        }

        public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = new User()
            {
                Name = request.Username,
                Password = request.Password,
            };
           return await this.authentacationService.CreateUserAsync(user);
        }
    }
}
