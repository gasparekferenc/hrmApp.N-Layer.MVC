using FluentValidation;
using hrmApp.Web.DTO;

namespace hrmApp.Web.Validators
{
    public class OrganizationDTOValidator : AbstractValidator<OrganizationDTO>
    {
        public OrganizationDTOValidator()
        {
            RuleFor(x => x.OrganizationName)
                .NotEmpty().WithMessage("Kötelező!")
                .MaximumLength(50).WithMessage("Maximum {MaxLength} karakter.");

            RuleFor(x => x.City)
                .MaximumLength(100).WithMessage("Maximum {MaxLength} karakter.");

            RuleFor(x => x.Address)
                .MaximumLength(30).WithMessage("Maximum {MaxLength} karakter.");

            RuleFor(x => x.ContactEmail)
                .EmailAddress().WithMessage("Érvénytelen formátum.");

            RuleFor(x => x.ContactPhone)
                .MaximumLength(30).WithMessage("Maximum {MaxLength} karakter.");

            RuleFor(x => x.Description)
            .MaximumLength(1024).WithMessage("Maximum {MaxLength} karakter.");

            RuleFor(x => x.IsActive)
                .NotNull().WithMessage("Kötelező!");

        }
    }
}