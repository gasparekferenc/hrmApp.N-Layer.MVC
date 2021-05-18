using FluentValidation;
using hrmApp.Web.ViewModels.EmployeeViewModels;

namespace hrmApp.Web.Validators
{
    public class NewEmployeeViewModelValidator : AbstractValidator<NewEmployeeViewModel>
    {
        public NewEmployeeViewModelValidator()
        {
            RuleFor(x => x.SSNumber)
                .NotEmpty().WithMessage("Kötelező!")
                .MaximumLength(9).WithMessage("Maximum {MaxLength} karakter.")
                .Must(ssn => CommonValidators.CheckTAJ(ssn) == 0).WithMessage("Hibás TAJ szám!");
            // TODO: reguláris kifejezés...

            RuleFor(x => x.SurName)
                .NotEmpty().WithMessage("Kötelező!")
                .MaximumLength(20).WithMessage("Maximum {MaxLength} karakter.");

            RuleFor(x => x.ForeName)
                .NotEmpty().WithMessage("Kötelező!")
                .MaximumLength(20).WithMessage("Maximum {MaxLength} karakter.");

            RuleFor(x => x.Birthplace)
                .NotEmpty().WithMessage("Kötelező!")
                .MaximumLength(50).WithMessage("Maximum {MaxLength} karakter.");
        }
    }
}