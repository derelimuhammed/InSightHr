using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ONION_PROJECT.APPLICATION.Services.TokenServices
{
    internal class TokenService:ITokenService
    {
        private readonly IConfiguration _config;
        public TokenService(IConfiguration config)
        {
            _config = config;
        }
        public string CreateToken(int UserID)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenSecretKey = _config.GetSection("Token:SecretKey").Value ?? string.Empty;
            var tokenExpireMinutes = int.Parse(_config.GetSection("Token:ExpireMinutes").Value ?? "0");
            var key = Encoding.ASCII.GetBytes(tokenSecretKey);
          var tokendecriptır = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("UserID", UserID.ToString()),
                    //new Claim("Role", RoleType.Admin.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(tokenExpireMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokendecriptır);
            string TokenString = tokenHandler.WriteToken(token);
            return TokenString;
        }
        public TokenVm GetUserInfo(HttpRequest request)
        {
            var responseModel = new TokenVm();
            string jwtToken = request.Headers["Authorization"].ToString().Remove(0, 7);
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken decodedToken = tokenHandler.ReadJwtToken(jwtToken);
            //responseModel.Role = decodedToken.Payload?["Role"].ToString() ?? string.Empty;
            //responseModel.ID = int.Parse(decodedToken.Payload?["UserID"].ToString() ?? "0");
            return responseModel;
        }


    }
}
