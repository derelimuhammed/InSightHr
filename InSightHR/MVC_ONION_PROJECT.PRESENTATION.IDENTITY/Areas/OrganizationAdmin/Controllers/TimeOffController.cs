using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MVC_ONION_PROJECT.APPLICATION.Services.TimeOffServices;
using MVC_ONION_PROJECT.DOMAIN.Utilities.Results;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.TimeOffVms;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.EmployeeVms;
using MVC_ONION_PROJECT.APPLICATION.Services.EmployeeServicces;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.EmployeeDtos;
using MVC_ONION_PROJECT.DOMAIN.ENUMS;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.TimeOffDtos;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.AssetCategoryDtos;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.AssetCategoryVms;
using MVC_ONION_PROJECT.APPLICATION.Services.EmailServices;
using MVC_ONION_PROJECT.APPLICATION.Services.EnumHelpers;
using MVC_ONION_PROJECT.APPLICATION.Services.TimeOffTypeService;
using MVC_ONION_PROJECT.APPLICATION.Services.EmailHelper;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Controllers
{
    public class TimeOffController : OrganizationAdminBaseController
    {
        private readonly IEmailService _emailService;
        private readonly ITimeOffService _timeOffService;
        private readonly ITimeOffTypeService _timeOffTypeService;
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;
        private readonly IEnumHelperService _enumHelperService;
        private readonly IEmailHelper _emailHelper;


        public TimeOffController(ITimeOffService timeOffService, IMapper mapper, IEmployeeService employeeService, IEmailService emailService, IEnumHelperService enumHelperService, ITimeOffTypeService timeOffTypeService, IEmailHelper emailHelper)
        {
            _timeOffService = timeOffService;
            _mapper = mapper;
            _employeeService = employeeService;
            _emailService = emailService;
            _enumHelperService = enumHelperService;
            _timeOffTypeService = timeOffTypeService;
            _emailHelper = emailHelper;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _timeOffService.GetAllAsync();
            
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                return View(_mapper.Map<List<TimeOffListVm>>(result.Data));
            }
            var filterResult = result.Data.FindAll(x => x.ReturnStatus == DOMAIN.ENUMS.ReturnStatus.Pending);
            var model = _mapper.Map<List<TimeOffListVm>>(filterResult);
            foreach (var item in model)
            {
                var employee=await _employeeService.GetByidentityIdAsync(Guid.Parse(item.CreatedBy));
                item.Employee = employee.Data;
                TimeSpan difference = item.EndTime - item.StartTime;
                item.TimeSpanDay = difference.Days+1;
                var timeOffType= await _timeOffTypeService.GetByIdAsync(Guid.Parse(item.TimeOffTypeId));
                item.TimeOffTypeName = timeOffType.Data.Name;
            }
            return View(model);
        }
        [HttpGet("OrganizationAdmin/TimeOff/GetFilter/{filter?}")]
        public async Task<IActionResult> GetFilter(string filter)
        {
            var result = await _timeOffService.GetAllAsync();
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                return View(_mapper.Map<List<TimeOffListVm>>(result.Data));
            }
            var model = _mapper.Map<List<TimeOffListVm>>(result.Data);
            foreach (var item in model)
            {
                var employee = await _employeeService.GetByidentityIdAsync(Guid.Parse(item.CreatedBy));
                item.Employee = employee.Data;
                TimeSpan difference = item.EndTime - item.StartTime;
                item.TimeSpanDay = difference.Days;
            }
            if (filter == "Hepsi")
                return PartialView("_TimeOffListPartial", model);
            var filtermodel = model.FindAll(x => _enumHelperService.GetDisplayName(x.ReturnStatus) == filter).ToList();
           
            SuccessNoty(result.Message);
            return PartialView("_TimeOffListPartial", filtermodel);
        }
        [HttpPost("OrganizationAdmin/TimeOff/SetTimeOff/{Id}/{returnStatusstring}")]
        public async Task SetTimeOff(string Id, string returnStatusstring)
        {
            ReturnStatus? returnStatus = null;
            foreach (ReturnStatus status in Enum.GetValues(typeof(ReturnStatus)))
            {
                if (_enumHelperService.GetDisplayName(status) == returnStatusstring)
                {
                    returnStatus = status;
                    break;
                }
            }
            if (returnStatus == null)
            {
                return;
            }
            var timeOff = await _timeOffService.GetByIdAsync(Guid.Parse(Id));
            if (!ModelState.IsValid)
            {
                return;
            }
            var timeOffEditdto = _mapper.Map<TimeOffUpdateDto>(timeOff.Data);
            timeOffEditdto.ReturnStatus = returnStatus??ReturnStatus.Returned;
            var result = await _timeOffService.UpdateAsync(timeOffEditdto);
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                return;
            }
            if (timeOff.Data.ReturnStatus!=returnStatus)
            {
                var identityuser = await _employeeService.GetByidentityIdAsync(Guid.Parse(timeOff.Data.CreatedBy));

                var mail = await _emailHelper.IzinGuncellemeMail(result.Data.StartTime, result.Data.EndTime, returnStatus);

             await _emailService.SendMail($"{result.Data.StartTime}-{result.Data.EndTime} Arasındaki izin İsteminiz güncellenmiştir", mail, identityuser.Data.Email);
            }
            return;
        }
        [HttpGet("OrganizationAdmin/TimeOff/RejectedResponse/{Id}")]
        public async Task<IActionResult> RejectedResponse(string Id)
        {
            var result = await _timeOffService.GetByIdAsync(Guid.Parse(Id));
            if (result.Data.ReturnStatus==ReturnStatus.Rejected)
            {
                ErrorNoty("Reddedilmiş bir izni tekrardan rededemezsiniz");
                return View(_mapper.Map<TimeOffRejectedUpdateVm>(result.Data));
            }
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                return View(_mapper.Map<TimeOffRejectedUpdateVm>(result.Data));
            }
           
            return View(_mapper.Map<TimeOffRejectedUpdateVm>(result.Data));
        }
        [HttpPost]
        public async Task<IActionResult> RejectedResponse(TimeOffRejectedUpdateVm model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var TimeOffEditDto = _mapper.Map<TimeOffRejectedUpdateDto>(model);
            TimeOffEditDto.ReturnStatus= ReturnStatus.Rejected;
            var result = await _timeOffService.UpdateRecetedAsync(TimeOffEditDto);
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                return View(model);
            }
            var identityuser = await _employeeService.GetByidentityIdAsync(Guid.Parse(result.Data.CreatedBy));

            var mail = await _emailHelper.IzinGuncellemeMail(result.Data.StartTime, result.Data.EndTime, TimeOffEditDto.ReturnStatus);

           await _emailService.SendMail($"{result.Data.StartTime}-{result.Data.EndTime} Arasındaki izin İsteminiz güncellenmiştir", mail, identityuser.Data.Email);
            SuccessNoty(result.Message);
            return RedirectToAction(nameof(Index));
        }
    }
}
