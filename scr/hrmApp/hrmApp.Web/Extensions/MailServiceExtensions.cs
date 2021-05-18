using System.Text.Encodings.Web;
using System.Threading.Tasks;
using hrmApp.Core.Models;
using hrmApp.Core.Services;

namespace hrmApp.Web.Extensions
{
    public static class MailServiceExtensions
    {
        public static Task SendEmailConfirmationAsync(this IMailService mailService,
                                                    string toName,
                                                    string toEmail, string callbackUrl)
        {
            return mailService.SendEmailAsync(new MailRequest
            {
                ToEmail = toEmail,
                Subject = "Regisztráció megerőstése - hrmApp",
                Body = $"Tisztelt {toName}!" +
                        "<br />" +
                        "<br />" +
                        "Erre a linkre kattintva erősítse meg a hrmApp hozzáférését: " +
                        $"<a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>link</a>." +
                        "<br />" +
                        "<br />" +
                        "Üdvözlettel, hrmApp"
                        // TODO: röviden elmagyarázni mi a feladat a következőkben, és hányszor,
                        // illetve meddig használható ez az email.
                        // Ha ez sikertelenül telik el mi, akkor mi a teendő.
            });
        }

        public static Task SendResetPasswordAsync(this IMailService mailService,
                                                    string toName,
                                                    string toEmail, string callbackUrl)
        {
            return mailService.SendEmailAsync(new MailRequest
            {
                ToEmail = toEmail,
                Subject = "Jelszó megerőstése - hrmApp",
                Body = $"Tisztelt {toName}!" +
                        "<br />" +
                        "<br />" +
                        "Erre a linkre kattintva visszaállíthatja hrmApp hozzáférésének jelszavát: " +
                        $"<a href='{callbackUrl}'>link</a>." +
                        "<br />" +
                        "<br />" +
                        "Üdvözlettel, hrmApp"
            });
        }
    }
}
