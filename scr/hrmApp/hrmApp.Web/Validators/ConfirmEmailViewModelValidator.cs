using FluentValidation;
using hrmApp.Web.ViewModels.AccountViewModels;

namespace hrmApp.Web.Validators
{
    public class ConfirmEmailViewModelValidator : AbstractValidator<ConfirmEmailViewModel>
    {
        public ConfirmEmailViewModelValidator()
        {


            RuleFor(x => x.GDPRConfirmed)
                .Equal(true).WithMessage("Az alkalmazás használatához kötelező elfogadni!");

            RuleFor(x => x.TermOfUseConfirmed)
                .Equal(true).WithMessage("Az alkalmazás használatához kötelező elfogadni!");

        }
    }
}