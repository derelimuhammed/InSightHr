using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC_ONION_PROJECT.APPLICATION.Services.AdvancePaymentService;
using MVC_ONION_PROJECT.APPLICATION.Services.DepartmentService;
using MVC_ONION_PROJECT.APPLICATION.Services.EmployeeServicces;
using MVC_ONION_PROJECT.APPLICATION.Services.ReimbursementService;
using MVC_ONION_PROJECT.APPLICATION.Services.TimeOffServices;
using MVC_ONION_PROJECT.APPLICATION.Services.TimeOffTypeService;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.Employee.Models.EmployeeVms;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.AdvanceVms;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.EmployeeVms;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.IndexVms;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.ReimbursementVm;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.TimeOffVms;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Vms;
using System.Security.Claims;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Controllers
{
    public class HomeController : OrganizationAdminBaseController
    {

        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;
        private readonly IDepartmentService _departmentService;
        private readonly Microsoft.AspNetCore.Identity.UserManager<IdentityUser> _userManager;
        private readonly IReimbursementService _reimbursementService;
        private readonly ITimeOffService _timeOffService;
        private readonly IAdvancePaymentService _advancePaymentService;
        private readonly ITimeOffTypeService _timeOffTypeService;

        public HomeController(SignInManager<IdentityUser> signInManager, Microsoft.AspNetCore.Identity.UserManager<IdentityUser> userManager, IEmployeeService employeeService, IHttpContextAccessor contextAccessor, IMapper mapper, IDepartmentService departmentService, IReimbursementService reimbursementService, ITimeOffService timeOffService, IAdvancePaymentService advancePaymentService, ITimeOffTypeService timeOffTypeService)
        {
            _signInManager = signInManager;
            _contextAccessor = contextAccessor;
            _employeeService = employeeService;
            _mapper = mapper;
            _departmentService = departmentService;
            _userManager = userManager;
            _reimbursementService = reimbursementService;
            _timeOffService = timeOffService;
            _advancePaymentService = advancePaymentService;
            _timeOffTypeService = timeOffTypeService;
        }

        public async Task<IActionResult> Index()
        {
            var model = new IndexVm()
            {
                AdvanceListVmTake5 = await GetAdvancePaymentIndexList(),
                ReimbursementListVmTake5 = await GetReimbursementIndexList(),
                TimeOffListVmTake5 = await GetTimeOffIndexList(),
            };
            var EmployeeList = await _employeeService.GetEmployeesInOrganization();
            model.EmployeeCount = EmployeeList.Data.Count();
            var timeoff = await _timeOffService.GetAllAsync();
            if (timeoff.Data != null)
            {
                var numberOfPeopleOnLeave = timeoff.Data.Where(izin =>
      DateTime.Now >= izin.StartTime && DateTime.Now <= izin.EndTime);
                model.NumberOfPeopleOnLeave = numberOfPeopleOnLeave.Count();
            }
            if (model.AdvanceListVmTake5 == null)
                ErrorNoty("Avans isteği yoktur");
            if (model.TimeOffListVmTake5 == null)
                ErrorNoty("İzin isteği yoktur");
            if (model.ReimbursementListVmTake5 == null)
                ErrorNoty("Masraf isteği yoktur");
            return View(model);

        }
        public async Task<IActionResult> Profile()
        {
            var userId = _contextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var employees = await _employeeService.GetAllAsync();
            var employee = employees.Data.FirstOrDefault(x => x.IdentityId.ToString() == userId);
            if (employee == null)
            {
                ErrorNoty("Kullanıcı bulunamadı.");
                return View();
            }
            var result = await _employeeService.GetByIdAsync(employee.Id);
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                return View(_mapper.Map<EmployeeEmployeeDetailsVm>(result.Data));
            }
            SuccessNoty(result.Message);
            var employeeDetailVm = _mapper.Map<EmployeeEmployeeDetailsVm>(result.Data);
            var department = await _departmentService.GetByIdAsync(result.Data.DepartmentId);
            employeeDetailVm.DepartmentName = department.Data.DepartmentName;
            return View(employeeDetailVm);
        }
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var userId = _contextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.ChangePasswordAsync(user, vm.OldPassword, vm.NewPassword);
            if (!result.Succeeded)
            {
                ErrorNoty("8 Karakter,1 Sayı ve 1 Büyük Harf Olacak Şekilde Yeni Şifre Belirleyin");
                return View(vm);
            }
            SuccessNoty("Şifre Başarıyla Güncellendi.");
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }
        public async Task<List<ReimbursementListVm>?> GetReimbursementIndexList()
        {
            var getReimbursementIndexList = await _reimbursementService.GetAllAsync();
            if (!getReimbursementIndexList.IsSuccess)
                return null;
            var getReimbursementIndextake5List = getReimbursementIndexList.Data.Take(5);
            var getReimbursementIndextake5ListVm = _mapper.Map<List<ReimbursementListVm>>(getReimbursementIndextake5List);
            foreach (var item in getReimbursementIndextake5ListVm)
            {
                var employee = await _employeeService.GetByIdAsync(item.EmployeeId);
                item.Employee = employee.Data;
            }
            return getReimbursementIndextake5ListVm;
        }
        public async Task<List<TimeOffListVm>?> GetTimeOffIndexList()
        {
            var getTimeOffIndexList = await _timeOffService.GetAllAsync();
            if (!getTimeOffIndexList.IsSuccess)
                return null;
            var getTimeOffIndexIndextake5List = getTimeOffIndexList.Data.Take(5);
            var getTimeOffIIndextake5ListVm = _mapper.Map<List<TimeOffListVm>>(getTimeOffIndexIndextake5List);
            foreach (var item in getTimeOffIIndextake5ListVm)
            {
                var employee = await _employeeService.GetByidentityIdAsync(Guid.Parse(item.CreatedBy));
                item.Employee = employee.Data;
                TimeSpan difference = item.EndTime - item.StartTime;
                item.TimeSpanDay = difference.Days + 1;
                var timeOffType = await _timeOffTypeService.GetByIdAsync(Guid.Parse(item.TimeOffTypeId));
                item.TimeOffTypeName = timeOffType.Data.Name;
            }
            return getTimeOffIIndextake5ListVm;
        }
        public async Task<List<AdvanceListVm>?> GetAdvancePaymentIndexList()
        {
            var getAdvancePaymentIndexList = await _advancePaymentService.GetAllAsync();
            if (!getAdvancePaymentIndexList.IsSuccess)
                return null;
            var getAdvancePaymentIndextake5List = getAdvancePaymentIndexList.Data.Take(5);
            var getAdvancePaymentIndextake5ListVm = _mapper.Map<List<AdvanceListVm>>(getAdvancePaymentIndextake5List);
            foreach (var item in getAdvancePaymentIndextake5ListVm)
            {
                var employee = await _employeeService.GetByIdAsync(item.EmployeeId);
                item.Employee = employee.Data;
            }

            return getAdvancePaymentIndextake5ListVm;
        }
    }
}
