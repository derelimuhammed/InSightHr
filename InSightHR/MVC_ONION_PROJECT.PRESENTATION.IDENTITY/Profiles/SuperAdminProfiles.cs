using AutoMapper;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.EmployeeDtos;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.OrganizationDtos;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.PackageDtos;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.EmployeeVms;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.SuperAdmin.Models.OrganizationAdminVms;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.SuperAdmin.Models.OrganizationVms;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.SuperAdmin.Models.Package;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.SuperAdmin.Models.SuperAdminVm;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Profiles
{
    public class SuperAdminProfiles:Profile
    {
        public SuperAdminProfiles()
        {
            #region Organization Profiles
            CreateMap<OrganizationCreateVm, OrganizationCreateDto>();
            CreateMap<OrganizationListDto, OrganizationListVm>();
            CreateMap<OrganizationUpdateVm, OrganizationUpdateDto>();
            CreateMap<OrganizationDto, OrganizationUpdateVm>();
            CreateMap<OrganizationDto, OrganizationDetailsVm>();
            #endregion
            #region Package Profiles
            CreateMap<PackageListDto, PackageListVm>();
            CreateMap<PackageCreateVm, PackageCreateDto>();
            CreateMap<PackageUpdateVm, PackageUpdateDto>();
            CreateMap<PackageDto, PackageUpdateVm>();
            #endregion
            #region SuperAdmin Profiles
            CreateMap<EmployeeDto, SuperAdminDetailsVm>();

            #endregion
        }
    }
}
