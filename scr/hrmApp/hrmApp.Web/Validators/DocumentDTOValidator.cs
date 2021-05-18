using FluentValidation;
using hrmApp.Web.DTO;

namespace hrmApp.Web.Validators
{
    public class DocumentDTOValidator : AbstractValidator<DocumentDTO>
    {
        public DocumentDTOValidator()
        {
            RuleFor(x => x.DocDisplayName)
                .NotEmpty().WithMessage("Kötelező!")
                .MaximumLength(50).WithMessage("Maximum {MaxLength} karakter.");

            RuleFor(x => x.DocPath)
                .NotEmpty().WithMessage("Kötelező!")
                .MaximumLength(255).WithMessage("Maximum {MaxLength} karakter.");

            RuleFor(x => x.UploadedTimeStamp)
                .NotNull().WithMessage("Kötelező!");

            RuleFor(x => x.Description)
            .MaximumLength(1024).WithMessage("Maximum {MaxLength} karakter.");

            // ???
            RuleFor(x => x.EmployeeId)
                .NotEmpty().WithMessage("Kötelező!");

            RuleFor(x => x.DocTypeId)
                .NotEmpty().WithMessage("Kötelező!");

        }

    }
}
