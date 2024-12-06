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
        Task SendVerificationCodeAsync(string email, string verificationCode);
        Task AddVerificationCodeAsync(EmailVerification emailVerification);
        Task<EmailVerification> GetVerificationCodeAsync(string code);
        Task MarkCodeAsUsedAsync(string verificationCode);
    }

}
