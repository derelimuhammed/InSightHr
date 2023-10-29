using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MVC_ONION_PROJECT.INFRASTRUCTURE.APPCONTEXT;
using MVC_ONION_PROJECT.INFRASTRUCTURE.Repositories.Concretes;
using MVC_ONION_PROJECT.INFRASTRUCTURE.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ONION_PROJECT.INFRASTRUCTURE.EXTENSION
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureService (this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDBContext>(options => {
                options.UseLazyLoadingProxies();
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IEmployeeDebitRepository, EmployeeDebitRepository>();
            services.AddScoped<IEmployeeSalaryRepository, EmployeeSalaryRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            services.AddScoped<ITimeOffRepository, TimeOffRepository>();
            services.AddScoped<ITimeOffTypeRepository, TimeOffTypeRepository>();
            services.AddScoped<IAssetCategoryRepository, AssetCategoryRepository>();
            services.AddScoped<IOrgAssetRepository, OrgAssetRepository>();
            services.AddScoped<IAdvancePaymentRepository, AdvancePaymentRepository>();
            services.AddScoped<IPackageRepository, PackageRepository>();
            services.AddScoped<IReimbursementRepository, ReimbursementRepository>();
            return services;
        }
    }
}
