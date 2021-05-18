using FluentValidation;
using hrmApp.Web.DTO;

namespace hrmApp.Web.Validators
{
    public class DocTypeDTOValidator : AbstractValidator<DocTypeDTO>
    {
        public DocTypeDTOValidator()
        {
            RuleFor(x => x.TypeName)
                .NotEmpty().WithMessage("Kötelező!")
                .MaximumLength(40).WithMessage("Maximum {MaxLength} karakter.");

            RuleFor(x => x.MandatoryElement)
                .NotNull().WithMessage("Kötelező!");

            RuleFor(x => x.Description)
            .MaximumLength(1024).WithMessage("Maximum {MaxLength} karakter.");

            RuleFor(x => x.IsActive)
                .NotNull().WithMessage("Kötelező!");

            RuleFor(x => x.PreferOrder)
                .NotEmpty().WithMessage("Kötelező!")
                .InclusiveBetween(0, 500).WithMessage("{From}-{To} közötti szám.");
        }
    }
}
