using FluentValidation;
using hrmApp.Web.ViewModels.ComponentsViewModels;
using Microsoft.AspNetCore.Http;

namespace hrmApp.Web.Validators
{
    public class DocumentsViewModelValidator : AbstractValidator<DocumentsViewModel>
    {
        public DocumentsViewModelValidator()
        {
            RuleFor(x => x.AttachedFile).SetValidator(new FileValidator());
        }

        #region FileValidator : AbstractValidator<IFormFile>            
        public class FileValidator : AbstractValidator<IFormFile>
        {
            public FileValidator()
            {
                RuleFor(x => x.Length).NotNull().LessThanOrEqualTo(2 * 1024 * 1024) // 2 MB
                    .WithMessage("A file mérete maximum 2MB lehet.");

                RuleFor(x => x.ContentType)
                    .NotNull().Must(x => x.Equals("application/pdf"))
                    .WithMessage("A file típus nem megfelelő! Kizárólag .PDF engedélyezett.");
            }
        }
        #endregion
    }
}
