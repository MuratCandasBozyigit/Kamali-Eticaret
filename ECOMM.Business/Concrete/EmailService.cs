using ECOMM.Business.Abstract;
using ECOMM.Core.Models;
using ECOMM.Data;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using Microsoft.EntityFrameworkCore; // Bu namespace'i ekleyin
using System.Net;

public class SmtpEmailService : IEmailService
{
    private readonly ApplicationDBContext _context;
    private readonly IConfiguration _configuration;

    public SmtpEmailService(ApplicationDBContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        // SMTP ayarları
        var smtpClient = new SmtpClient(_configuration["EmailSettings:SmtpServer"])
        {
            Port = int.Parse(_configuration["EmailSettings:SmtpPort"]),
            Credentials = new NetworkCredential(_configuration["EmailSettings:SmtpUser"], _configuration["EmailSettings:SmtpPassword"]),
            EnableSsl = true,
        };

        // E-posta mesajı oluşturma
        var mailMessage = new MailMessage
        {
            From = new MailAddress(_configuration["EmailSettings:FromEmail"]),
            Subject = subject,
            Body = body,
            IsBodyHtml = true, // HTML formatta e-posta gönderimi
        };

        mailMessage.To.Add(toEmail);

        // E-postayı gönderme
        await smtpClient.SendMailAsync(mailMessage);
    }

    public async Task AddVerificationCodeAsync(EmailVerification emailVerification)
    {
        await _context.EmailVerifications.AddAsync(emailVerification);
        await _context.SaveChangesAsync();
    }

    public async Task<EmailVerification> GetVerificationCodeAsync(string code)
    {
        return await _context.EmailVerifications
            .Where(v => v.VerificationCode == code && v.ExpirationTime > DateTime.Now)
            .FirstOrDefaultAsync();
    }
}
