using System.Threading.Tasks;
using hrmApp.Core.Models;


namespace hrmApp.Core.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}

