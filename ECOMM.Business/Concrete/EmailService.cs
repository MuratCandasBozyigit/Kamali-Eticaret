using ECOMM.Business.Abstract;
using ECOMM.Core.Models;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using System;
using System.Threading.Tasks;

namespace ECOMM.Business.Concrete
{
    public class EmailService : IEmailService
    {
        // SMTP ayarları sabit olarak burada tanımlanıyor
        private const string SmtpServer = "smtp.gmail.com";
        private const int SmtpPort = 587;
        private const string SmtpUser = "cayperisi33@gmail.com";
        private const string SmtpPassword = "ujmwpsmddumwpzeb";
        private const string FromEmail = "cayperisi33@gmail.com";

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
        public async Task AddVerificationCodeAsync(EmailVerification verification)
        {
            // Örnek olarak bir doğrulama kodunu kaydetme
            Console.WriteLine("Doğrulama kodu veritabanına kaydedildi:");
            Console.WriteLine($"Kod: {verification.VerificationCode}");
            Console.WriteLine($"Email: {verification.Email}");
            Console.WriteLine($"Expiration: {verification.ExpirationTime}");
        }

        // Veritabanından doğrulama kodunu getiren metod
        public async Task<EmailVerification> GetVerificationCodeAsync(string verificationCode)
        {
            // Örnek olarak doğrulama kodunu veritabanından getirme
            Console.WriteLine($"Doğrulama kodu '{verificationCode}' getirildi.");
            return new EmailVerification
            {
                VerificationCode = verificationCode,
                Email = "test@example.com",
                ExpirationTime = DateTime.Now.AddMinutes(5),
                IsUsed = false
            };
        }

        // Doğrulama kodunu "kullanılmış" olarak işaretleyen metod
        public async Task MarkCodeAsUsedAsync(string verificationCode)
        {
            // Örnek olarak doğrulama kodunu kullanılmış olarak işaretleme
            Console.WriteLine($"Doğrulama kodu '{verificationCode}' kullanıldı olarak işaretlendi.");
        }
    }

    // Email doğrulama için model sınıfı
    public class EmailVerification
    {
        public int Id { get; set; }
        public string UserId { get; set; } // Kullanıcıya ait ID
         public DateTime CreatedAt { get; set; } // Kodu oluşturma zamanı
        public string VerificationCode { get; set; }
        public string Email { get; set; }
        public DateTime ExpirationTime { get; set; }
        public bool IsUsed { get; set; }
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
