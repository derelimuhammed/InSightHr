using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ONION_PROJECT.APPLICATION.Services.PasswordCreateServices
{
    public interface IPasswordCreateService
    {
        Task<string> GeneratePasswordAsync(int passwordLength);
    }
}
