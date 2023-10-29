using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ONION_PROJECT.APPLICATION.Services.EmailServices
{
    public interface IEmailService
    {
        Task SendMail(string head, string content, string MailAdresi);
    }
}
