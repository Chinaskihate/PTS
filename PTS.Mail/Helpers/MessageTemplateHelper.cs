using PTS.Backend.Utils;
using PTS.Mail.Models;
using System.Web;

namespace PTS.Mail.Helpers;
public static class MessageTemplateHelper
{
    public static MailMessage CreateRecoverPasswordMessage(
        string userName,
        string userEmail,
        string userId,
        string recoverCode)
    {
        return new MailMessage
        {
            ToEmail = userEmail,
            ToName = userName,
            Subject = "Password recovery code",
            Body = $"Dear {userName}, here is your verification <a href=\"{SD.PasswordRecoveryLink}/{userId}/{HttpUtility.UrlEncode(recoverCode)}\">link</a> for password recovery"
        };
    }
}
