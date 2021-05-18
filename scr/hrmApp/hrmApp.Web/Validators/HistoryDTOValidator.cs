using FluentValidation;
using hrmApp.Web.DTO;

namespace hrmApp.Web.Validators
{
    public class HistoryDTOValidator : AbstractValidator<HistoryDTO>
    {
        public HistoryDTOValidator()
        {
            RuleFor(x => x.Entry)
                .NotEmpty().WithMessage("Kötelező!")
                .MaximumLength(1024).WithMessage("Maximum {MaxLength} karakter.");

            RuleFor(x => x.AppUserEntry)
                .NotEmpty().WithMessage("Kötelező!");

            RuleFor(x => x.IsReminder)
               .NotEmpty().WithMessage("Kötelező!");

            RuleFor(x => x.EmployeeId)
               .NotEmpty().WithMessage("Kötelező!");

            RuleFor(x => x.ApplicationUserId)
               .NotEmpty().WithMessage("Kötelező!");
        }
    }
}
