using Notification.Domain.Entities;
using Notification.Domain.Interfaces;
using System.Net;
using System.Net.Mail;

namespace Notification.Infrastructure.Providers;

public class EmailProvider : INotificationProvider
{
    public bool IsEnabled(UserEntity user)
    {
        if (user.EmailEnable)
        {
            return true;
        }

        return false;
    }

    public async Task<(bool Success, string Error)> SendAsync(UserEntity user, string message, CancellationToken cancellationToken)
    {
        try
        {
            var email = user.Email;

            using var client = new SmtpClient("", 2525)
            {
                Credentials = new NetworkCredential("user", "pass"),
                EnableSsl = true
            };

            var mailMessage = new MailMessage("", email)
            {
                Subject = "",
                Body = message
            };

            await client.SendMailAsync(mailMessage, cancellationToken);
            return (true, "Ok");
        }
        catch (Exception ex)
        {
            return (false, $"{ex.Message} {ex.StackTrace}");
        }
    }
}
