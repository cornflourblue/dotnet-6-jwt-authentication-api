namespace WebApi.Aplication.Handlers.Validator
{
    using FluentValidation;
    using WebApi.Infrascture.Command;
    public class AuthenticateRequestCommandValidator : AbstractValidator<AuthenticateRequestCommand>
    {
        public AuthenticateRequestCommandValidator()
        {
            RuleFor(request => request.Username)
                .NotEmpty().WithMessage("Name is mandory");
            RuleFor(request => request.Password)
                .NotEmpty().WithMessage("Password is mandatory");
        }
    }
}
