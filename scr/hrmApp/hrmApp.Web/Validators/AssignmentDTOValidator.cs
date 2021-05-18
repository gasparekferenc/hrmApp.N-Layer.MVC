using FluentValidation;
using hrmApp.Web.DTO;

namespace hrmApp.Web.Validators
{
    public class AssignmentDTOValidator : AbstractValidator<AssignmentDTO>
    {
        public AssignmentDTOValidator()
        {
            RuleFor(x => x.ApplicationUserId)
                .NotEmpty().WithMessage("Kötelező!");

            RuleFor(x => x.OrganizationId)
                .NotEmpty().WithMessage("Kötelező!");
        }

    }
}
