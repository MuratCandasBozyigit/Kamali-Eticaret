using ECOMM.Business.Abstract;
using ECOMM.Core.Models;
using MimeKit;
using MailKit.Net.Smtp;
using System.Threading.Tasks;

namespace ECOMM.Business.Concrete
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpHost = "smtp.gmail.com";
        private readonly int _smtpPort = 587;
        private readonly string _smtpUsername = "dizikesitleri3307@gmail.com";
        private readonly string _smtpPassword = "9212arda";

        // Doğrulama kodunu gönderen metod
        public async Task SendVerificationCodeAsync(string email, string verificationCode)
        {
            var subject = "Your Verification Code";
            var body = $"Your verification code is {verificationCode}";

            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Your Name", _smtpUsername)); // Ad ve e-posta adresi
            emailMessage.To.Add(new MailboxAddress("", email)); // Ad boş bırakılabilir, sadece e-posta adresi verilir
            emailMessage.Subject = subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = body };
            emailMessage.Body = bodyBuilder.ToMessageBody();

            // MailKit SmtpClient kullanımı
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_smtpHost, _smtpPort, false);
                await client.AuthenticateAsync(_smtpUsername, _smtpPassword);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }

        // Doğrulama kodunu veritabanına ekleyen metod
        public async Task AddVerificationCodeAsync(EmailVerification verification)
        {
            // Burada doğrulama kodunu veritabanına kaydedebilirsiniz
        }

        // Veritabanından doğrulama kodunu getiren metod
        public async Task<EmailVerification> GetVerificationCodeAsync(string verificationCode)
        {
            // Burada doğrulama kodunu veritabanından alabilirsiniz
            return null; // Örnek olarak null dönüyoruz
        }

        // Doğrulama kodunu "kullanılmış" olarak işaretleyen metod
        public async Task MarkCodeAsUsedAsync(string verificationCode)
        {
            // Burada doğrulama kodunu "kullanılmış" olarak işaretleyebilirsiniz
        }
    }
}
