using MailKit.Net.Smtp;
using MimeKit;
using PTS.Mail.Models;
using PTS.Mail.Settings;

namespace PTS.Mail.Services;
public class SmtpMailService(SmtpSettings settings) : IEmailService
{
    private readonly SmtpSettings _settings = settings;

    public async Task SendEmailAsync(MailMessage msg)
    {
        var mimeMsg = new MimeMessage();
        mimeMsg.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
        mimeMsg.To.Add(new MailboxAddress(msg.ToName, msg.ToEmail));
        mimeMsg.Subject = msg.Subject;
        mimeMsg.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = msg.Body
        };

        using var client = new SmtpClient();
        await client.ConnectAsync(_settings.Host, _settings.Port, true);
        await client.AuthenticateAsync(_settings.SenderEmail, _settings.SenderPassword);
        await client.SendAsync(mimeMsg);
        await client.DisconnectAsync(true);
    }
}
