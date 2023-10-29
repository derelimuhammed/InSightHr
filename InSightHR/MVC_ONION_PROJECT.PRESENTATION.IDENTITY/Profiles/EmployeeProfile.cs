using AutoMapper;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.AdvancePayment;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.AdvancePaymentDtos;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.EmployeeDtos;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.ReimbursementDtos;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.TimeOffDtos;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.TimeOffTypeDtos;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.Employee.Models.EmployeeAdvanceVms;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.Employee.Models.EmployeeReimbursementVms;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.Employee.Models.EmployeeVms;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.Employee.Models.TimeOffTypeVms;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.Employee.Models.TimeOffVms;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.EmployeeVms;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Profiles
{
    public class EmployeeProfile:Profile
    {
        public EmployeeProfile()
        {
            #region Employee Profiles
            CreateMap<EmployeeDto, EmployeeUpdateVm>();
            CreateMap<EmployeeDto, EmployeeEmployeeDetailsVm>();
            #endregion
            #region EmployeeTimeOff
            CreateMap<TimeOffCreateVm, TimeOffCreateDto>();
            CreateMap<TimeOffListDto, TimeOffListVm>();
            CreateMap<TimeOffUpdateVm, TimeOffUpdateDto>();
            CreateMap<TimeOffDto, TimeOffUpdateVm>();
            CreateMap<TimeOffDto, TimeOffDetailVm>();
            #endregion
            #region TimeOffType
            CreateMap<TimeOffTypeCreateVm, TimeOffTypeCreateDto>();
            CreateMap<TimeOffTypeListDto, TimeOffTypeListVm>();
            CreateMap<TimeOffTypeUpdateVm, TimeOffTypeUpdateDto>();
            CreateMap<TimeOffTypeDto, TimeOffTypeUpdateVm>();
            CreateMap<TimeOffTypeDto, TimeOffTypeDetailVm>();
            #endregion
            #region AdvancePayment
            CreateMap<EmployeeAdvanceCreateVm, AdvancePaymentCreateDto>();
            CreateMap<AdvancePaymentListDto, EmployeeAdvanceListVm>();
            CreateMap<EmployeeAdvanceUpdateVm, AdvancePaymentUpdateDto>();
            CreateMap<AdvancePaymentDto, EmployeeAdvanceUpdateVm>();
            CreateMap<AdvancePaymentDto, EmployeeAdvanceDetailVm>();
            #endregion
            #region Reimbursement
            CreateMap<EmployeeReimbursementCreateVm, ReimbursementCreateDto>();
            CreateMap<ReimbursementListDto, EmployeeReimbursementListVm>();
            CreateMap<EmployeeReimbursementUpdateVm, ReimbursementUpdateDto>();
            CreateMap<ReimbursementDto, EmployeeReimbursementUpdateVm>();
            CreateMap<ReimbursementDto, EmployeeReimbursementDetailsVm>();
            #endregion
        }
    }
}
