using FluentValidation;
using hrmApp.Web.DTO;

namespace hrmApp.Web.Validators
{
    public class JobDTOValidator : AbstractValidator<JobDTO>
    {
        public JobDTOValidator()
        {
            RuleFor(x => x.JobName)
                .NotEmpty().WithMessage("Kötelező!")
                .MaximumLength(20).WithMessage("Maximum {MaxLength} karakter.");

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
