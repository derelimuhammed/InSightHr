using Microsoft.AspNetCore.Identity;
using MVC_ONION_PROJECT.DOMAIN.ENUMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ONION_PROJECT.APPLICATION.Services.EmailHelper
{
    public interface IEmailHelper
    {
        Task SendPasswordVerificationEmailAsync(string subject, string contents, string controller, string action, IdentityUser user, string password);
        Task SendVerificationEmailAsync(string subject, string contents, string controller, string action, IdentityUser user);
        Task<string> AvansMailIstek(string name, string surname, int amount);
        Task<string> AvansGuncellemeMail(DateTime createdDate, int amount, ReturnStatus? status);
        Task<string> HarcamaMailIstek(string name, string surname, decimal amount);
        Task<string> HarcamaGuncellemeMail(DateTime createdDate, decimal amount, PaymentStatus? status);
        Task<string> IzinMailIstek(string name, string surname, int amount);
        Task<string> IzinGuncellemeMail(DateTime startDate, DateTime endDate, ReturnStatus? status);
        Task<string> ZimmetAtamaMail(string debitName);
        Task<bool> SendContactFormMessageAsync(string subject, string content, string email);
    }
}
