using FluentValidation;
using hrmApp.Web.DTO;

namespace hrmApp.Web.Validators
{
    public class EmployeeDTOValidator : AbstractValidator<EmployeeDTO>
    {
        public EmployeeDTOValidator()
        {
            RuleFor(x => x.SurName)
                .NotEmpty().WithMessage("Kötelező!")
                .MaximumLength(20).WithMessage("Maximum {MaxLength} karakter.");

            RuleFor(x => x.ForeName)
                .NotEmpty().WithMessage("Kötelező!")
                .MaximumLength(20).WithMessage("Maximum {MaxLength} karakter.");

            RuleFor(x => x.Birthplace)
                .NotEmpty().WithMessage("Kötelező!")
                .MaximumLength(50).WithMessage("Maximum {MaxLength} karakter.");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Kötelező!");

            RuleFor(x => x.PermPostCode)
                .MaximumLength(4).WithMessage("Maximum {MaxLength} karakter.");

            RuleFor(x => x.PermCity)
                .MaximumLength(50).WithMessage("Maximum {MaxLength} karakter.");

            RuleFor(x => x.PermAddress)
                .MaximumLength(100).WithMessage("Maximum {MaxLength} karakter.");

            RuleFor(x => x.ResPostCode)
                .MaximumLength(4).WithMessage("Maximum {MaxLength} karakter.");

            RuleFor(x => x.ResCity)
                .MaximumLength(50).WithMessage("Maximum {MaxLength} karakter.");

            RuleFor(x => x.ResAddress)
                .MaximumLength(100).WithMessage("Maximum {MaxLength} karakter.");

            RuleFor(x => x.EduLevel)
                .MaximumLength(50).WithMessage("Maximum {MaxLength} karakter.");

            RuleFor(x => x.EduDocId)
                .MaximumLength(50).WithMessage("Maximum {MaxLength} karakter.");

            RuleFor(x => x.EduInstitute)
                .MaximumLength(50).WithMessage("Maximum {MaxLength} karakter.");

            RuleFor(x => x.MothersName)
                .MaximumLength(100).WithMessage("Maximum {MaxLength} karakter.");

            RuleFor(x => x.SSNumber)
                .NotEmpty().WithMessage("Kötelező!")
                .MaximumLength(9).WithMessage("Maximum {MaxLength} karakter.")
                .Must(ssn => CommonValidators.CheckTAJ(ssn) == 0).WithMessage("Hibás TAJ szám!");

            RuleFor(x => x.TINumber)
                .MaximumLength(10).WithMessage("Maximum {MaxLength} karakter.");

            RuleFor(x => x.BANumber)
                .MaximumLength(26).WithMessage("Maximum {MaxLength} karakter.");

            RuleFor(x => x.Description)
            .MaximumLength(1024).WithMessage("Maximum {MaxLength} karakter.");

            RuleFor(x => x.PhoneNumber)
                .MaximumLength(30).WithMessage("Maximum {MaxLength} karakter.");

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Érvénytelen formátum.");

            RuleFor(x => x.Description)
                .MaximumLength(1024).WithMessage("Maximum {MaxLength} karakter.");

            RuleFor(x => x.EndDate)
            .GreaterThan(p => p.StartDate).WithMessage("Nem lehet korábbi, mint a Belépés dátuma.");

        }

    }
}