using FluentValidation;
using WebApi.Context;

namespace WebApi.Aplication.Handlers.Validator
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(request => request.Iduser)
               .NotNull().WithMessage("Null");
           
        }
    }
}
