﻿namespace PTS.Mail.Settings;
public class SmtpSettings
{
    public string Host { get; set; }
    public int Port { get; set; }
    public string SenderName { get; set; }
    public string SenderEmail { get; set; }
    public string SenderPassword { get; set; }
}
