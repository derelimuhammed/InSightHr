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
using MVC_ONION_PROJECT.APPLICATION.DTo_s.TimeOffDtos;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.TimeOffTypeDtos;
using MVC_ONION_PROJECT.DOMAIN.ENTITIES;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.Employee.Models.EmployeeAdvanceVms;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.Employee.Models.EmployeeVms;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.Employee.Models.TimeOffTypeVms;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.Employee.Models.TimeOffVms;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.EmployeeVms;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.TimeOffVms;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.AssetCategoryVms;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.DepartmentVms;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.EmployeeDebitVms;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.OrgAssetVms;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.SalaryVms;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.AdvanceVms;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.ReimbursementDtos;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.ReimbursementVm;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Profiles
{
    public class OrganizationAdminProfiles:Profile
    {
        public OrganizationAdminProfiles()
        {
            #region Employee Profiles
            CreateMap<EmployeeCreateVm, EmployeeCreateDto>();
            CreateMap<EmployeeListDto, EmployeeListVm>();         
            CreateMap<EmployeeDto, EmployeeDetailsVm>();
            CreateMap<EmployeeDto, EmployeeListVm>();
            CreateMap<EmployeeUpdateVm, EmployeeUpdateDto>();
            #endregion

            #region Depratment Profiles
            CreateMap<DepartmentCreateVm, DepartmentCreateDto>();
            CreateMap<DepartmentListDto, DepartmentListVm>();
            CreateMap<DepartmentUpdateVms, DepartmentUpdateDto>();
            CreateMap<DepartmentDto, DepartmentUpdateVms>();
            CreateMap<DepartmentDto, DepartmentDetailsVm>();
            CreateMap<EmployeeListDto, EmployeeListByDepartmentVm>();
            #endregion

            #region EmployeeDebit Profiles
            CreateMap<EmployeeDebitCreateVm, EmployeeDebitCreateDto>();
            CreateMap<EmployeeDebitListDto, EmployeeDebitListVm>();
            CreateMap<EmployeeDebitDto, EmployeeDebitUpdateVm>();
            CreateMap<EmployeeDebitUpdateVm, EmployeeDebitUpdateDto>();
            CreateMap<EmployeeDebitDto, EmployeeDebitDetailsVm>();
            #endregion

            #region Salary Profiles
            CreateMap<SalaryCreateVm, SalaryCreateDto>();
            CreateMap<SalaryListDto, SalaryListVm>();
            CreateMap<SalaryUpdateVm, SalaryUpdateDto>();
            CreateMap<SalaryDto, SalaryUpdateVm>();
            #endregion

            #region TimeOff
            //CreateMap<TimeOffCreateVm, TimeOffCreateDto>();
            CreateMap<TimeOffListDto, Areas.OrganizationAdmin.Models.TimeOffVms.TimeOffListVm>();
            //CreateMap<TimeOffUpdateVm, TimeOffUpdateDto>();
            CreateMap<TimeOffDto, TimeOffUpdateDto>();
            CreateMap<TimeOffDto, TimeOffRejectedUpdateVm>();
            CreateMap<TimeOffRejectedUpdateVm, TimeOffRejectedUpdateDto>();
            //CreateMap<TimeOffDto, TimeOffDetailVm>();
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

            #region AdvancePayment
            //CreateMap<TimeOffCreateVm, TimeOffCreateDto>();
            CreateMap<AdvancePaymentListDto, AdvanceListVm>();
            //CreateMap<TimeOffUpdateVm, TimeOffUpdateDto>();
            CreateMap<AdvancePaymentDto, AdvancePaymentRejectedUpdateVm>();
            CreateMap<AdvancePaymentRejectedUpdateVm, AdvancePaymentRejectedUpdateDto>();
            CreateMap<AdvancePaymentRejectedUpdateDto, AdvancePayment>();
            //CreateMap<TimeOffDto, TimeOffDetailVm>();
            #endregion
            #region Reimbursement
            //CreateMap<TimeOffCreateVm, TimeOffCreateDto>();
            CreateMap<ReimbursementListDto, ReimbursementListVm>();
            //CreateMap<TimeOffUpdateVm, TimeOffUpdateDto>();
            CreateMap<ReimbursementDto, ReimbursementRejectedUpdateVm>();
            CreateMap<ReimbursementDto, ReimbursementDetailsVm>();
            CreateMap<ReimbursementRejectedUpdateVm, ReimbursementRejectedUpdateDto>();
            CreateMap<ReimbursementRejectedUpdateDto, Reimbursement>();
            //CreateMap<TimeOffDto, TimeOffDetailVm>();
            #endregion

        }
    }
}
