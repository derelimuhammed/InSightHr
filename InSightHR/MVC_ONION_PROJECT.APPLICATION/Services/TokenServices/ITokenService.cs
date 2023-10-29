using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ONION_PROJECT.APPLICATION.Services.TokenServices
{
    public interface ITokenService
    {
        string CreateToken(int UserID);
        TokenVm GetUserInfo(HttpRequest request);
    }
}
