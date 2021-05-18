using FluentValidation;
using hrmApp.Web.DTO;

namespace hrmApp.Web.Validators
{
    public class ProjectOrganizationDTOValidator : AbstractValidator<ProjectOrganizationDTO>
    {
        public ProjectOrganizationDTOValidator()
        {
            RuleFor(x => x.ProjectId)
                .NotEmpty().WithMessage("Kötelező!");

            RuleFor(x => x.OrganizationId)
                .NotEmpty().WithMessage("Kötelező!");
        }
    }
}
