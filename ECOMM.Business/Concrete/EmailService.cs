using ECOMM.Business.Abstract;
using ECOMM.Core.Models;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using System.Threading.Tasks;
using System;
using ECOMM.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ECOMM.Business.Concrete
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;
        private readonly ApplicationDBContext _context;

        public EmailService(ApplicationDBContext context, IOptions<SmtpSettings> settings)
        {
            _context = context;

            // Ayarları yapılandırma
            _smtpHost = settings.Value.Host;
            _smtpPort = settings.Value.Port;
            _smtpUsername = settings.Value.Username;
            _smtpPassword = settings.Value.Password;
        }

        // Doğrulama kodunu gönderen metod
        public async Task SendVerificationCodeAsync(string email, string verificationCode)
        {
            var emailMessage = new MimeMessage
            {
                Subject = "Your Verification Code"
            };

            emailMessage.From.Add(new MailboxAddress("Your App Name", _smtpUsername));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Body = new BodyBuilder
            {
                HtmlBody = $"<p>Your verification code is <strong>{verificationCode}</strong></p>"
            }.ToMessageBody();

            try
            {
                using (var client = new SmtpClient())
                {
                    client.Timeout = 10000; // 10 saniye
                    await client.ConnectAsync(_smtpHost, _smtpPort, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(_smtpUsername, _smtpPassword);

                    await client.SendAsync(emailMessage);
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                // Hataları loglamak için bir araç kullanılabilir (örn. Serilog, NLog)
                Console.WriteLine($"Hata: {ex.Message}");
                throw;
            }
        }

        // Doğrulama kodunu veritabanına ekleyen metod
        public async Task AddVerificationCodeAsync(EmailVerification verification)
        {
            _context.EmailVerifications.Add(verification);
            await _context.SaveChangesAsync();
        }

        // Veritabanından doğrulama kodunu getiren metod
        public async Task<EmailVerification> GetVerificationCodeAsync(string verificationCode)
        {
            return await _context.EmailVerifications
                .FirstOrDefaultAsync(v => v.VerificationCode == verificationCode && !v.IsUsed && v.ExpirationTime > DateTime.Now);
        }

        // Doğrulama kodunu "kullanılmış" olarak işaretleyen metod
        public async Task MarkCodeAsUsedAsync(string verificationCode)
        {
            var verification = await _context.EmailVerifications
                .FirstOrDefaultAsync(v => v.VerificationCode == verificationCode && !v.IsUsed && v.ExpirationTime > DateTime.Now);

            if (verification != null)
            {
                verification.IsUsed = true;
                await _context.SaveChangesAsync();
            }
        }
    }

    // Email ayarları için kullanılacak yapılandırma sınıfı
    public class SmtpSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
