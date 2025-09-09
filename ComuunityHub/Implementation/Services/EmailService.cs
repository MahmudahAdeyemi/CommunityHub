using ComuunityHub.Interfaces.Services;
using ComuunityHub.Models;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;

namespace ComuunityHub.Implementation.Services;

public class EmailService : IEmailService
{
    private readonly EmailSettings  _emailSettings;
    public EmailService(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }
    public async Task SendEmailAsync(string toEmail, string subject, string body, bool isHtml = true)
    {
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
        email.To.Add(MailboxAddress.Parse(toEmail));
        email.Subject = subject;

        email.Body = isHtml
            ? new TextPart("html") { Text = body }
            : new TextPart("plain") { Text = body };
        using var smtp = new SmtpClient();
        var secureSocket = SecureSocketOptions.Auto;
        await smtp.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.Port, secureSocket);
        await smtp.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}