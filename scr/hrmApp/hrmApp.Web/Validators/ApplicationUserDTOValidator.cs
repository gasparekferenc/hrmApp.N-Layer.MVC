using FluentValidation;
using hrmApp.Web.DTO;

namespace hrmApp.Web.Validators
{
    public class ApplicationUserDTOValidator : AbstractValidator<ApplicationUserDTO>
    {
        public ApplicationUserDTOValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Kötelező!")
                .MaximumLength(256).WithMessage("Maximum {MaxLength} karakter.");

            RuleFor(x => x.SurName)
                .NotEmpty().WithMessage("Kötelező!")
                .MaximumLength(20).WithMessage("Maximum {MaxLength} karakter.");

            RuleFor(x => x.ForeName)
                .NotEmpty().WithMessage("Kötelező!")
                .MaximumLength(20).WithMessage("Maximum {MaxLength} karakter.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Kötelező!")
                .MaximumLength(256).WithMessage("Maximum {MaxLength} karakter.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Kötelező!")
                .EmailAddress().WithMessage("Érvénytelen formátum.");

            RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Maximum {MaxLength} karakter.");
        }
    }
}
