using Microsoft.Extensions.DependencyInjection;
using MVC_Onion_Project.Application.Services.AccountService;
using MVC_ONION_PROJECT.APPLICATION.Services.AddImageServices;
using MVC_ONION_PROJECT.APPLICATION.Services.AdvancePaymentService;
using MVC_ONION_PROJECT.APPLICATION.Services.AssetCategoryService;
using MVC_ONION_PROJECT.APPLICATION.Services.DepartmentService;
using MVC_ONION_PROJECT.APPLICATION.Services.EmailHelper;
using MVC_ONION_PROJECT.APPLICATION.Services.EmailServices;
using MVC_ONION_PROJECT.APPLICATION.Services.EmployeeDebitService;
using MVC_ONION_PROJECT.APPLICATION.Services.EmployeeServicces;
using MVC_ONION_PROJECT.APPLICATION.Services.EnumHelpers;
using MVC_ONION_PROJECT.APPLICATION.Services.OrganizationServices;
using MVC_ONION_PROJECT.APPLICATION.Services.OrgAssetService;
using MVC_ONION_PROJECT.APPLICATION.Services.PackageService;
using MVC_ONION_PROJECT.APPLICATION.Services.PasswordCreateServices;
using MVC_ONION_PROJECT.APPLICATION.Services.ReimbursementService;
using MVC_ONION_PROJECT.APPLICATION.Services.SalaryService;
using MVC_ONION_PROJECT.APPLICATION.Services.TimeOffServices;
using MVC_ONION_PROJECT.APPLICATION.Services.TimeOffTypeService;
using MVC_ONION_PROJECT.APPLICATION.Services.TokenServices;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace MVC_ONION_PROJECT.APPLICATION.Extension
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

			services.AddScoped<IAccountService, AccountService>();
			services.AddScoped<IEmployeeService, EmployeeService>();
			services.AddScoped<IEmailService, EmailService>();
			services.AddScoped<IEmailHelper, EmailHelper>();
			services.AddScoped<IDepartmentService, DepartmentService>();
			services.AddScoped<IAddImageService, AddImageService>();
			services.AddScoped<IEmployeeDebitService, EmployeeDebitService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ISalaryService, SalaryService>();
            services.AddScoped<IOrganizationService, OrganizationService>();
            services.AddScoped<ITimeOffService, TimeOffService>();
            services.AddScoped<ITimeOffTypeService, TimeOffTypeService>();
            services.AddScoped<IAssetCategoryService, AssetCategoryService>();
            services.AddScoped<IOrgAssetService, OrgAssetService>();
            services.AddScoped<IPasswordCreateService, PasswordCreateService>();
            services.AddScoped<IAdvancePaymentService, AdvancePaymentService>();
            services.AddScoped<IEnumHelperService, EnumHelperService>();
            services.AddScoped<IPackageService, PackageService>();
            services.AddScoped<IReimbursementService, ReimbursementService>();
            return services;
        }
    }
}
