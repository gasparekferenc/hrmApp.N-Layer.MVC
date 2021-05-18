using FluentValidation;
using hrmApp.Web.DTO;

namespace hrmApp.Web.Validators
{
    public class POEmployeeDTOValidator : AbstractValidator<POEmployeeDTO>
    {
        public POEmployeeDTOValidator()
        {
            RuleFor(x => x.ProjectOrganizationId)
                .NotEmpty().WithMessage("Kötelező!");

            RuleFor(x => x.EmployeeId)
                .NotEmpty().WithMessage("Kötelező!");
        }
    }
}
