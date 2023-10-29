using AutoMapper;
using MVC_ONION_PROJECT.APPLICATION.DTo_s;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.AdvancePayment;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.AdvancePaymentDtos;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.AssetCategoryDtos;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.DepartmentDtos;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.EmployeeDebitDtos;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.EmployeeDtos;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.OrgAssetDtos;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.SalaryDtos;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.Employee.Models.EmployeeVms;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.AssetCategoryVms;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.DepartmentVms;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.EmployeeDebitVms;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.EmployeeVms;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.OrgAssetVms;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.SalaryVms;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Profiles
{
    public class OrganizationProfiles:Profile
    {
        public OrganizationProfiles()
        {
            #region Employee Profiles
            CreateMap<EmployeeCreateVm, EmployeeCreateDto>();
            CreateMap<EmployeeListDto, EmployeeListVm>();
            CreateMap<EmployeeDto, EmployeeUpdateVm>();
            CreateMap<EmployeeDto, EmployeeDetailsVm>();
            CreateMap<EmployeeUpdateVm, EmployeeUpdateDto>();
            #endregion

            #region Depratment Profiles
            CreateMap<DepartmentCreateVm, DepartmentCreateDto>();
            CreateMap<DepartmentListDto, DepartmentListVm>();
            CreateMap<DepartmentUpdateVms, DepartmentUpdateDto>();
            CreateMap<DepartmentDto, DepartmentUpdateVms>();
            CreateMap<DepartmentDto, DepartmentDetailsVm>();
            #endregion

            #region EmployeeDebit Profiles
            CreateMap<EmployeeDebitCreateVm, EmployeeDebitCreateDto>();
            CreateMap<EmployeeDebitListDto, EmployeeDebitListVm>();
            CreateMap<EmployeeDebitDto, EmployeeDebitUpdateVm>();
            CreateMap<EmployeeDebitUpdateVm, EmployeeDebitUpdateDto>();
            #endregion

            #region Salary Profiles
            CreateMap<SalaryCreateVm, SalaryCreateDto>();
            CreateMap<SalaryListDto, SalaryListVm>();
            CreateMap<SalaryUpdateVm, SalaryUpdateDto>();
            CreateMap<SalaryDto, SalaryUpdateVm>();
            #endregion

            #region AssetCategory Profiles
            CreateMap<AssetCategoryCreateVm, AssetCategoryCreateDto>();
            CreateMap<AssetCategoryListDto, AssetCategoryListVm>();
            CreateMap<AssetCategoryUpdateVm, AssetCategoryUpdateDto>();
            CreateMap<AssetCategoryDto, AssetCategoryUpdateVm>();
            #endregion

            #region OrgAsset Profiles
            CreateMap<OrgAssetCreateVm, OrgAssetCreateDto>();
            CreateMap<OrgAssetListDto, OrgAssetListVm>();
            CreateMap<OrgAssetUpdateVm, OrgAssetUpdateDto>();
            CreateMap<OrgAssetDto, OrgAssetUpdateVm>();
            CreateMap<OrgAssetDto, OrgAssetDetailsVm>();
            #endregion
     
        }
    }
}
