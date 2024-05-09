namespace PTS.Mail.Models;
public class MailMessage
{
    public string Subject { get; set; }
    public string Body { get; set; }
    public string? FromEmail { get; set; }
    public string? FromName { get; set; }
    public string? ToName { get; set; }
    public string ToEmail { get; set; }
}
