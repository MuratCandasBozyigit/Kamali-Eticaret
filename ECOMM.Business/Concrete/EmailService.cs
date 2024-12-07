﻿using ECOMM.Business.Abstract;
using ECOMM.Core.Models;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using ECOMM.Data;

namespace ECOMM.Business.Concrete
{
    public class EmailService : IEmailService
    {
        private readonly ApplicationDBContext _context;

        // SMTP ayarları sabit olarak burada tanımlanıyor
        private const string SmtpServer = "smtp.gmail.com";
        private const int SmtpPort = 587;
        private const string SmtpUser = "cayperisi33@gmail.com";
        private const string SmtpPassword = "ujmwpsmddumwpzeb";
        private const string FromEmail = "cayperisi33@gmail.com";

        // EmailService constructor - Inject ApplicationDbContext
        public EmailService(ApplicationDBContext context)
        {
            _context = context;
        }

        // Doğrulama kodunu gönderen metod
        public async Task SendVerificationCodeAsync(string email, string verificationCode)
        {
            var emailMessage = new MimeMessage
            {
                Subject = "Your Verification Code"
            };

            emailMessage.From.Add(new MailboxAddress("Your App Name", FromEmail));
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
                    await client.ConnectAsync(SmtpServer, SmtpPort, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(SmtpUser, SmtpPassword);

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
        public async Task AddVerificationCodeAsync(Core.Models.EmailVerification verification)
        {
            // Veritabanına doğrulama kodunu kaydediyoruz.
            await _context.EmailVerifications.AddAsync(verification);
            await _context.SaveChangesAsync();
        }

        // Veritabanından doğrulama kodunu getiren metod
        public async Task<EmailVerification> GetVerificationCodeAsync(string verificationCode)
        {
            // Veritabanından doğrulama kodunu getiriyoruz.
            var verification = await _context.EmailVerifications
                .FirstOrDefaultAsync(v => v.VerificationCode == verificationCode && !v.IsUsed);

            return verification;
        }

        // Doğrulama kodunu "kullanılmış" olarak işaretleyen metod
        public async Task MarkCodeAsUsedAsync(string verificationCode)
        {
            var verification = await _context.EmailVerifications
                .FirstOrDefaultAsync(v => v.VerificationCode == verificationCode);

            if (verification != null)
            {
                verification.IsUsed = true;
                await _context.SaveChangesAsync();
            }
        }
    }

  


    // Email servisi için abstract sınıf (arayüz)
    public interface IEmailService
    {
        Task SendVerificationCodeAsync(string email, string verificationCode);
        Task AddVerificationCodeAsync(EmailVerification verification);
        Task<EmailVerification> GetVerificationCodeAsync(string verificationCode);
        Task MarkCodeAsUsedAsync(string verificationCode);
    }

}
