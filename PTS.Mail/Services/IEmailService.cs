using PTS.Mail.Models;

namespace PTS.Mail.Services;
public interface IEmailService
{
    public Task SendEmailAsync(MailMessage msg);
}
