using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.DepartmentDtos;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.TimeOffDtos;
using MVC_ONION_PROJECT.APPLICATION.Services.EmailHelper;
using MVC_ONION_PROJECT.APPLICATION.Services.EmailServices;
using MVC_ONION_PROJECT.APPLICATION.Services.EmployeeServicces;
using MVC_ONION_PROJECT.APPLICATION.Services.EnumHelpers;
using MVC_ONION_PROJECT.APPLICATION.Services.TimeOffServices;
using MVC_ONION_PROJECT.APPLICATION.Services.TimeOffTypeService;
using MVC_ONION_PROJECT.DOMAIN.ENUMS;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.Employee.Models.TimeOffVms;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.DepartmentVms;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.TimeOffVms;
using System.Security.Claims;
using TimeOffListVm = MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.Employee.Models.TimeOffVms.TimeOffListVm;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.Employee.Controllers
{
    public class EmployeTimeOffController : EmployeeBaseController
    {
        private readonly ITimeOffService _timeOffService;
        private readonly ITimeOffTypeService _timeOffTypeService;
        private readonly IMapper _mapper;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IEmployeeService _employeeService;
        private readonly IEmailService _emailService;
        private readonly IEnumHelperService _enumHelperService;
        private readonly IEmailHelper _emailHelper;

        public EmployeTimeOffController(ITimeOffService timeOffService, IMapper mapper, SignInManager<IdentityUser> signInManager, ITimeOffTypeService timeOffTypeService, IEmployeeService employeeService, IHttpContextAccessor contextAccessor, IEmailService emailService, IEnumHelperService enumHelperService, IEmailHelper emailHelper)
        {
            _timeOffService = timeOffService;
            _mapper = mapper;
            _signInManager = signInManager;
            _timeOffTypeService = timeOffTypeService;
            _employeeService = employeeService;
            _contextAccessor = contextAccessor;
            _emailService = emailService;
            _enumHelperService = enumHelperService;
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
            var userTimeOffList = result.Data.FindAll(x => x.CreatedBy == _signInManager.Context.User?.FindFirstValue(ClaimTypes.NameIdentifier));
            var userTimeOffListVM = _mapper.Map<List<TimeOffListVm>>(userTimeOffList);
            foreach (var item in userTimeOffListVM)
            {
                var timeOffTypeId = item.TimeOffTypeId;
                var timeOffType = await _timeOffTypeService.GetByIdAsync(timeOffTypeId);
                item.Name = timeOffType.Data.Name;
            }
            return View(userTimeOffListVM);
        }

        [HttpGet]
        public async Task<IActionResult> AddTimeOff()
        {
            TimeOffCreateVm vm = new TimeOffCreateVm()
            {
                TimeOffList = await GetTimeOffSelectListAsync(),
                StartTime = DateTime.Now,
                EndTime = DateTime.Now
            };
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> AddTimeOff(TimeOffCreateVm model, string returnStatusstring)
        {
            if (!ModelState.IsValid)
            {
                model.TimeOffList = await GetTimeOffSelectListAsync();
                return View(model);
            }
            
            var userId = _contextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var employee = await _employeeService.GetByidentityIdAsync(Guid.Parse(userId));
            model.EmployeeId = employee.Data.Id;
            model.ReturnStatus = DOMAIN.ENUMS.ReturnStatus.Pending;

            if (model.StartTime > model.EndTime)
            {
                ErrorNoty("Başlangıç tarihi bitiş tarihinden önce sonra olamaz.");
                model.TimeOffList = await GetTimeOffSelectListAsync();
                return View(model);
            }

            var addResult = await _timeOffService.AddAsync(_mapper.Map<TimeOffCreateDto>(model));
            if (!addResult.IsSuccess)
            {
                ErrorNoty(addResult.Message);
                model.TimeOffList = await GetTimeOffSelectListAsync();
                return View(model);
            }

            var mailTaslak = await _emailHelper.AvansMailIstek(employee.Data.Name, employee.Data.Surname, model.NumberOfDays);

            await _emailService.SendMail("Yıllık İzin Kullanımı Hk.",mailTaslak, "cem.sener@bilgeadamboost.com");
            SuccessNoty(addResult.Message);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var timeOff = await _timeOffService.GetByIdAsync(id);
            if (timeOff != null && (timeOff.Data.ReturnStatus == DOMAIN.ENUMS.ReturnStatus.Assigned || timeOff.Data.StartTime.Date < DateTime.Now.Date))
            {
                ErrorNoty("Bu izin talebi onaylanmış ve tarihi geçmiş, silinemez.");
                return RedirectToAction(nameof(Index));
            }

            var result = await _timeOffService.DeleteAsync(id);
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                return RedirectToAction(nameof(Index));
            }
            SuccessNoty(result.Message);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> EditTimeOff(Guid id)
        {
            var result = await _timeOffService.GetByIdAsync(id);
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                return View(_mapper.Map<TimeOffUpdateVm>(result.Data));
            }
            if (result.Data.ReturnStatus != DOMAIN.ENUMS.ReturnStatus.Pending)
            {
                ErrorNoty("Cevaplanmış talep değiştirilemez");
                return RedirectToAction(nameof(Index));
            }
            var timeOffEditVm = _mapper.Map<TimeOffUpdateVm>(result.Data);
            return View(timeOffEditVm);
        }

        [HttpPost]
        public async Task<IActionResult> EditTimeOff(TimeOffUpdateVm model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var timeOffEditDto = _mapper.Map<TimeOffUpdateDto>(model);
            timeOffEditDto.ReturnStatus = ReturnStatus.Pending;

            if (model.StartTime > model.EndTime)
            {
                ErrorNoty("Başlangıç tarihi bitiş tarihinden önce sonra olamaz.");
                return View(model);
            }
  
            var result = await _timeOffService.UpdateAsync(timeOffEditDto);
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                return View(model);
            }

            SuccessNoty(result.Message);
            var resultVm = _mapper.Map<TimeOffUpdateVm>(result.Data);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DetailsTimeOff(Guid id)
        {
            var result = await _timeOffService.GetByIdAsync(id);
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                return View(_mapper.Map<TimeOffDetailVm>(result.Data));
            }
            SuccessNoty(result.Message);
            var timeOffDetailVm = _mapper.Map<TimeOffDetailVm>(result.Data);
            return View(timeOffDetailVm);
        }

        private async Task<SelectList> GetTimeOffSelectListAsync()
        {
            var timeOffType = await _timeOffTypeService.GetAllAsync();

            return new SelectList(timeOffType.Data.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }), "Value", "Text");
        }
    }
}
