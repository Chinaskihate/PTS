using PTS.Mail.Models;

namespace PTS.Mail.Helpers;
public static class MessageTemplateHelper
{
    public static MailMessage CreateRecoverPasswordMessage(
        string userName,
        string userEmail,
        string recoverCode)
    {
        return new MailMessage
        {
            ToEmail = userEmail,
            ToName = userName,
            Subject = "Password recovery code",
            Body = $"Dear {userName}, here is your verification code for password recovery: {recoverCode}"
        };
    }
}
