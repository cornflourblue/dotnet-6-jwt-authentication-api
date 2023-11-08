namespace WebApi.Aplication.Handlers.Command
{
    using AutoMapper;
    using FluentValidation;
    using FluentValidation.Results;
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
            ValidationResult validationResult = validator.Validate(request);
            if (validationResult.IsValid)
            {
                var response = authentacationService.Authenticate(request);
                responseDTO = new AuthenticationTokenDto { Token = response.Token };
            }
            return Task.FromResult(responseDTO);
        }
    }
}
