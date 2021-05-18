using FluentValidation;
using hrmApp.Web.DTO;

namespace hrmApp.Web.Validators
{
    public class ProcessStatusDTOValidator : AbstractValidator<ProcessStatusDTO>
    {
        public ProcessStatusDTOValidator()
        {
            RuleFor(x => x.StatusName)
                .NotEmpty().WithMessage("Kötelező!")
                .MaximumLength(30).WithMessage("Maximum {MaxLength} karakter.");

            RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Maximum {MaxLength} karakter.");

            RuleFor(x => x.IsActive)
                .NotNull().WithMessage("Kötelező!");

            RuleFor(x => x.PreferOrder)
                .NotEmpty().WithMessage("Kötelező!")
                .InclusiveBetween(0, 500).WithMessage("{From}-{To} közötti szám.");
        }
    }
}
