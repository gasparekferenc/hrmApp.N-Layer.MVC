using FluentValidation;
using hrmApp.Web.DTO;

namespace hrmApp.Web.Validators
{
    public class ProjectDTOValidator : AbstractValidator<ProjectDTO>
    {
        public ProjectDTOValidator()
        {
            RuleFor(x => x.ProjectName)
                .NotEmpty().WithMessage("Kötelező!")
                .MaximumLength(20).WithMessage("Maximum {MaxLength} karakter.");

            RuleFor(x => x.NumberOfEmployee)
                .NotEmpty().WithMessage("Kötelező!")
                .InclusiveBetween(1, 500).WithMessage("{From}-{To} közötti szám.");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("Kötelező!");

            RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("Kötelező!")
            .GreaterThan(p => p.StartDate).WithMessage("Nem lehet korábbi, mint a kezdő dátum.");

            RuleFor(x => x.Description)
            .MaximumLength(1024).WithMessage("Maximum {MaxLength} karakter.");

            RuleFor(x => x.IsActive)
                .NotNull().WithMessage("Kötelező!");
        }

    }
}
