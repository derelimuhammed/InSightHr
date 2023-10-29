using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC_ONION_PROJECT.APPLICATION.Services.DepartmentService;
using MVC_ONION_PROJECT.APPLICATION.Services.EmployeeServicces;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.EmployeeVms;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.SuperAdmin.Models.SuperAdminVm;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Vms;
using System.Security.Claims;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.SuperAdmin.Controllers
{
    public class HomeController : SuperAdminBaseController
    {

        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;
        private readonly IDepartmentService _departmentService;
        private readonly Microsoft.AspNetCore.Identity.UserManager<IdentityUser> _userManager;

        public HomeController(SignInManager<IdentityUser> signInManager, Microsoft.AspNetCore.Identity.UserManager<IdentityUser> userManager, IEmployeeService employeeService, IHttpContextAccessor contextAccessor, IMapper mapper, IDepartmentService departmentService)
        {
            _signInManager = signInManager;
            _contextAccessor = contextAccessor;
            _employeeService = employeeService;
            _mapper = mapper;
            _departmentService = departmentService;
            _userManager= userManager;
        }

        public async Task<IActionResult> Index()
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
                return View(_mapper.Map<SuperAdminDetailsVm>(result.Data));
            }
            var employeeDetailVm = _mapper.Map<SuperAdminDetailsVm>(result.Data);
            var department = await _departmentService.GetByIdAsync(result.Data.DepartmentId);
            employeeDetailVm.DepartmentName = department.Data.DepartmentName;
            return View(employeeDetailVm);
        }

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }
        public async Task<IActionResult> ChangePassword()
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
    }
}
