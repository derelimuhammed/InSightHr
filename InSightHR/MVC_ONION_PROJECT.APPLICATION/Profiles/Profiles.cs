using AutoMapper;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.AdvancePayment;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.AdvancePaymentDtos;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.AssetCategoryDtos;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.DepartmentDtos;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.EmployeeDebitDtos;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.EmployeeDtos;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.OrganizationDtos;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.OrgAssetDtos;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.PackageDtos;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.ReimbursementDtos;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.SalaryDtos;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.TimeOffDtos;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.TimeOffTypeDtos;
using MVC_ONION_PROJECT.DOMAIN.ENTITIES;


namespace MVC_ONION_PROJECT.APPLICATION.Profiles
{
    public class Profiles : Profile
    {
        public Profiles()
        {
            #region Employee Profiles
            CreateMap<Employee, EmployeeDto>();
            CreateMap<Employee, EmployeeUpdateDto>().ReverseMap();
            CreateMap<EmployeeDto, EmployeeUpdateDto>();
            CreateMap<Employee, EmployeeListDto>();
            CreateMap<EmployeeCreateDto,Employee>();
            #endregion

            #region Department Profiles
            CreateMap<Department, DepartmentDto>();
            CreateMap<Department, DepartmentListDto>();
            CreateMap<Department, DepartmentUpdateDto>().ReverseMap();
            CreateMap<DepartmentCreateDto, Department>();
            #endregion

            #region EmployeeDebit Profiles
            CreateMap<EmployeeDebit, EmployeeDebitListDto>();
            CreateMap<EmployeeDebit, EmployeeDebitDto>();
            CreateMap<EmployeeDebitCreateDto, EmployeeDebit>();
            CreateMap<EmployeeDebit, EmployeeDebitDto>().ReverseMap();
            CreateMap<EmployeeDebitUpdateDto, EmployeeDebit>();
            #endregion

            #region Salary Profiles
            CreateMap<EmployeeSalary, SalaryDto>();
            CreateMap<EmployeeSalary, SalaryListDto>();
            CreateMap<EmployeeSalary, SalaryUpdateDto>().ReverseMap();
            CreateMap<SalaryCreateDto, EmployeeSalary>();
            #endregion

            #region Organization Profiles
            CreateMap<Organization, OrganizationListDto>();
            CreateMap<Organization, OrganizationDto>();
            CreateMap<OrganizationCreateDto, Organization>();
            CreateMap<OrganizationCreateDto, OrganizationDto>();
            CreateMap<OrganizationDto, OrganizationUpdateDto>();
            CreateMap<OrganizationUpdateDto, Organization>();
            #endregion

            #region AssetCategory Profiles
            CreateMap<AssetCategory, AssetCategoryDto>();
            CreateMap<AssetCategory, AssetCategoryListDto>();
            CreateMap<AssetCategory, AssetCategoryUpdateDto>().ReverseMap();
            CreateMap<AssetCategoryCreateDto, AssetCategory>();
            #endregion

            #region OrgAsset Profiles
            CreateMap<OrgAsset, OrgAssetDto>();
            CreateMap<OrgAsset, OrgAssetListDto>();
            CreateMap<OrgAsset, OrgAssetUpdateDto>().ReverseMap();
            CreateMap<OrgAssetCreateDto, OrgAsset>();
            #endregion

            #region TimeOffs
            CreateMap<TimeOff, TimeOffDto>();
            CreateMap<TimeOff, TimeOffListDto>();
            CreateMap<TimeOff, TimeOffUpdateDto>().ReverseMap();
            CreateMap<TimeOffRejectedUpdateDto, TimeOff>();
            CreateMap<TimeOffCreateDto, TimeOff>();
            #endregion

            #region TimeOffTypes
            CreateMap<TimeOffType, TimeOffTypeDto>();
            CreateMap<TimeOffType, TimeOffTypeListDto>();
            CreateMap<TimeOffType, TimeOffTypeUpdateDto>().ReverseMap();
            CreateMap<TimeOffTypeCreateDto, TimeOffType>();
            #endregion

            #region AdvancePayments
            CreateMap<AdvancePayment, AdvancePaymentDto>();
            CreateMap<AdvancePayment, AdvancePaymentListDto>();
            CreateMap<AdvancePayment, AdvancePaymentUpdateDto>().ReverseMap();
            CreateMap<AdvancePaymentCreateDto, AdvancePayment>();
            CreateMap<AdvancePaymentDto, AdvancePaymentUpdateDto>();
            #endregion
            #region Package
            CreateMap<Package, PackageDto>();
            CreateMap<Package, PackageListDto>();
            CreateMap<Package, PackageUpdateDto>().ReverseMap();
            CreateMap<PackageCreateDto, Package>();
            CreateMap<PackageDto, PackageUpdateDto>();
            #endregion
            #region Reimbursement
            CreateMap<Reimbursement, ReimbursementDto>();
            CreateMap<Reimbursement, ReimbursementListDto>();
            CreateMap<Reimbursement, ReimbursementUpdateDto>().ReverseMap();
            CreateMap<ReimbursementCreateDto, Reimbursement>();
            CreateMap<ReimbursementDto, ReimbursementUpdateDto>();
            CreateMap<ReimbursementDto, ReimbursementDetailsDto>();
            #endregion
        }
    }
}
