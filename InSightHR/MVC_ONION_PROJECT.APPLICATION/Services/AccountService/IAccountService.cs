using Microsoft.AspNetCore.Identity;
using MVC_ONION_PROJECT.DOMAIN.ENUMS;
using MVC_ONION_PROJECT.DOMAIN.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Onion_Project.Application.Services.AccountService
{
    public interface IAccountService
    {
        Task<bool> AnyAsync(Expression<Func<IdentityUser, bool>> expression);
        Task<IdentityUser?> FindByIdAsync(string identityId);
        Task<IdentityResult> CreateUserAsync(IdentityUser user,string password,Role role);
        Task<IdentityResult> DeleteUserAsync(string identityId);
        Task<Guid> GetUserIdAsync(Guid identityId, string role);
        Task<IdentityResult> UpdateUserAsync(IdentityUser user);
        public Task<string> OrnekKismiAl(string email);
        Task<string> GetOrganizationdAsync(Guid identityId);
        //Task<IResult> SendMail(string identityId);
    }
}
