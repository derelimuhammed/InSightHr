using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MVC_ONION_PROJECT.DOMAIN.ENTITIES;
using MVC_ONION_PROJECT.DOMAIN.ENUMS;
using MVC_ONION_PROJECT.DOMAIN.Utilities;
using MVC_ONION_PROJECT.DOMAIN.Utilities.Results;
using MVC_ONION_PROJECT.INFRASTRUCTURE.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Onion_Project.Application.Services.AccountService
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _employeeDepartmentRepository;
        private readonly IOrganizationRepository _organizationRepository;

        public AccountService(UserManager<IdentityUser> userManager, IEmployeeRepository employeeRepository, IDepartmentRepository employeeDepartmentRepository, IOrganizationRepository organizationRepository)
        {
            _userManager = userManager;
            _employeeRepository = employeeRepository;
            _employeeDepartmentRepository = employeeDepartmentRepository;
            _organizationRepository = organizationRepository;
        }



        public async Task<bool> AnyAsync(Expression<Func<IdentityUser, bool>> expression)
        {
            return await _userManager.Users.AnyAsync(expression);
        }

        public async Task<IdentityResult> CreateUserAsync(IdentityUser user, string password, Role role)
        {
           
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                return result;

            }
            return await _userManager.AddToRoleAsync(user, role.ToString());
        }

        public async Task<IdentityResult> DeleteUserAsync(string identityId)
        {
            var user = await _userManager.FindByIdAsync(identityId);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError()
                {
                    Code = "kullanıcı bulunamadı",
                    Description = "kullanıcı bulunamadı"

                });
            }
            return await _userManager.DeleteAsync(user);

        }

        public async Task<IdentityUser?> FindByIdAsync(string identityId)
        {
            return await _userManager.FindByIdAsync(identityId);
        }

        public async Task<Guid> GetUserIdAsync(Guid identityId, string role)
        {
            Employee? user = role switch
            {
                "Admin" => await _employeeRepository.GetByIdAsync(identityId),
            };
            return user is null ? Guid.NewGuid() : user.Id;
        }
        public async Task<string> GetOrganizationdAsync(Guid identityId)
        {
            var employees = await _employeeRepository.GetAllAsync();
            var employee= employees.FirstOrDefault(x=>x.IdentityId==identityId.ToString());
            if (employee!=null)
            {
                var department = await _employeeDepartmentRepository.GetByIdAsync(employee.DepartmentId);
                if (department != null)
                {
                    var organization=await _organizationRepository.GetByIdAsync(department.OrganizationId);
                    if (organization != null)
                    {
                        return organization.Id.ToString();
                    }
                    return "Organizasyon bulunamadı";
                }
                return "Organizasyon bulunamadı";
            }
            return "Organizasyon bulunamadı";
        }

        public async Task<IdentityResult> UpdateUserAsync(IdentityUser user)
        {
            return await _userManager.UpdateAsync(user);
        }

        public async Task<string> OrnekKismiAl(string email)
        {
            if (!string.IsNullOrWhiteSpace(email) && email.Contains("@"))
            {
                string[] parcalanmisEmail = email.Split('@');
                if (parcalanmisEmail.Length == 2 && !string.IsNullOrWhiteSpace(parcalanmisEmail[0]))
                {
                    return parcalanmisEmail[0];
                }
            }

            return string.Empty;
        }

        //public async Task<IResult> SendMail(string identityId)
        //{
            
        //}

    }
}
