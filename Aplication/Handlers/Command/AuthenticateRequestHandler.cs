namespace WebApi.Aplication.Handlers.Command
{
    using AutoMapper;
    using FluentValidation;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;
    using WebApi.Aplication.DTOs;
    using WebApi.Infrascture.Command;
    using WebApi.Services.Contracts;

    public class AuthenticateRequestHandler : IRequestHandler<AuthenticateRequestCommand, AuthenticationTokenDto>
    {
        private readonly IAuthenticationJWTService authentacationService;
        private readonly IValidator<AuthenticateRequestCommand> validator;
        private readonly IMapper mapper;

        public AuthenticateRequestHandler(IAuthenticationJWTService authentacationService,
            IValidator<AuthenticateRequestCommand> validator, IMapper mapper)
        {
            this.authentacationService = authentacationService;
            this.validator = validator;
            this.mapper = mapper;
        }

        public Task<AuthenticationTokenDto> Handle(AuthenticateRequestCommand request, CancellationToken cancellationToken)
        {
            var responseDTO = new AuthenticationTokenDto();
            var response = authentacationService.Authenticate(request);
            responseDTO = !string.IsNullOrEmpty(response.Token)
                ? new AuthenticationTokenDto { Token = response.Token } : responseDTO;
            return Task.FromResult(responseDTO);
        }
    }
}
