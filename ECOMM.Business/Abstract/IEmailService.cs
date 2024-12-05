using ECOMM.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECOMM.Business.Abstract
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
        Task AddVerificationCodeAsync(EmailVerification emailVerification);
        Task<EmailVerification> GetVerificationCodeAsync(string code);
    }

}
