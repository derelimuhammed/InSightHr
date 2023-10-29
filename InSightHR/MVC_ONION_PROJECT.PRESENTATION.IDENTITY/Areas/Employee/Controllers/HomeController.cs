using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC_ONION_PROJECT.APPLICATION.Services.DepartmentService;
using MVC_ONION_PROJECT.APPLICATION.Services.EmployeeServicces;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.Employee.Models.EmployeeVms;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.SuperAdmin.Models.SuperAdminVm;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Vms;
using System.Security.Claims;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.Employee.Controllers
{
    public class HomeController : EmployeeBaseController
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;
        private readonly IDepartmentService _departmentService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly Microsoft.AspNetCore.Identity.UserManager<IdentityUser> _userManager;


        public HomeController(IEmployeeService employeeService, IMapper mapper, IDepartmentService departmentService, IHttpContextAccessor contextAccessor, SignInManager<IdentityUser> signInManager, Microsoft.AspNetCore.Identity.UserManager<IdentityUser> userManager)
        {
            _employeeService = employeeService;
            _mapper = mapper;
            _departmentService = departmentService;
            _contextAccessor = contextAccessor;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _contextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var employees = await _employeeService.GetAllAsync();
            var employee=employees.Data.FirstOrDefault(x => x.IdentityId.ToString() == userId);
            if(employee == null)
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
        public  IActionResult ChangePassword()
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
    }
}
