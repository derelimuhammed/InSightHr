using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.DepartmentDtos;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.EmployeeDtos;
using MVC_ONION_PROJECT.APPLICATION.Services.AddImageServices;
using MVC_ONION_PROJECT.APPLICATION.Services.DepartmentService;
using MVC_ONION_PROJECT.APPLICATION.Services.EmailHelper;
using MVC_ONION_PROJECT.APPLICATION.Services.EmailServices;
using MVC_ONION_PROJECT.APPLICATION.Services.EmployeeDebitService;
using MVC_ONION_PROJECT.APPLICATION.Services.EmployeeServicces;
using MVC_ONION_PROJECT.APPLICATION.Services.OrganizationServices;
using MVC_ONION_PROJECT.APPLICATION.Services.OrgAssetService;
using MVC_ONION_PROJECT.APPLICATION.Services.PackageService;
using MVC_ONION_PROJECT.DOMAIN.ENTITIES;
using MVC_ONION_PROJECT.INFRASTRUCTURE.Migrations;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Controllers;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.DepartmentVms;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.EmployeeDebitVms;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.EmployeeVms;
using System.Diagnostics.Metrics;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.Encodings.Web;

namespace MVC_ONION_PROJECT.APPLICATION.Services.EmployeeServicces
{
    public class EmployeeController : OrganizationAdminBaseController
    {
        private readonly IEmployeeService _employeeService;
        private readonly IEmployeeDebitService _employeeDebitService;
        private readonly IDepartmentService _departmentService;
        private readonly IMapper _mapper;
        private readonly IAddImageService _addImageService;
        private readonly IOrgAssetService _orgAssetService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IOrganizationService _organizationService;
        private readonly IPackageService _packageService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EmployeeController(IEmployeeService employeeService, IDepartmentService departmentService, IMapper mapper, IAddImageService addImageService, UserManager<IdentityUser> userManager, IEmployeeDebitService employeeDebitService, IOrgAssetService orgAssetService, IPackageService packageService, IOrganizationService organizationService, IHttpContextAccessor httpContextAccessor)
        {
            _employeeService = employeeService;
            _departmentService = departmentService;
            _mapper = mapper;
            _addImageService = addImageService;
            _userManager = userManager;
            _employeeDebitService = employeeDebitService;
            _orgAssetService = orgAssetService;
            _packageService = packageService;
            _organizationService = organizationService;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet("OrganizationAdmin/Employee/index")]
        public async Task<IActionResult> Index()
        {

            var result = await _employeeService.GetEmployeesInOrganization();
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                return View(_mapper.Map<List<EmployeeListVm>>(result.Data));
            }
            var model = _mapper.Map<List<EmployeeListVm>>(result.Data);

            //SuccessNoty(result.Message);
            return View(model);
        }
        [HttpGet("OrganizationAdmin/Employee/GetFilter/{filter?}")]
        public async Task<IActionResult> GetFilter(string filter)
        {
            var result = await _employeeService.GetEmployeesInOrganization();
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                return View(_mapper.Map<List<EmployeeListVm>>(result.Data));
            }
            var model = _mapper.Map<List<EmployeeListVm>>(result.Data);
            if (filter == "Active")
            {
                var filtermodel = model.FindAll(x => x.IsActive == true).ToList();
                return PartialView("_EmployeeListPartial", filtermodel);
            }

            else if (filter == "Inactive")
            {
                var filtermodel = model.FindAll(x => x.IsActive == false).ToList();
                return PartialView("_EmployeeListPartial", filtermodel);
            }
            SuccessNoty(result.Message);
            return PartialView("_EmployeeListPartial", model);
        }

        [HttpGet]
        public async Task<IActionResult> AddEmployee()
        {
            EmployeeCreateVm vm = new EmployeeCreateVm();
           var selectlist= await GetDepartmentSelectListAsync();
            if (selectlist==null)
            {
                ErrorNoty("Departman Eklemeden Kişi Ekleyemezsiniz");
                return RedirectToAction("index");
            }
            vm.DepartmentList = selectlist;
            vm.RecruitmentDate = DateTime.Now;
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromServices] IEmailHelper _email, EmployeeCreateVm model)
        {
            if (!ModelState.IsValid)
            {
                model.DepartmentList = await GetDepartmentSelectListAsync();
                return View(model);
            }
            //resimde dahil olmak üzere service geçmesi lazım

            var organization = await _organizationService.GetByIdAsync(Guid.Parse(HttpContext.Session.GetString("OrganizationId")));
            var package = await _packageService.GetByIdAsync(organization.Data.PackageId);
            var usercoun = await _departmentService.GetByOrganizationCount(organization.Data.Id);
            if (package.Data.NumberOfUser < usercoun)
            {
                ErrorNoty("Kullanıcı Sayısını Aştınız için İşleminiz Başarız Gerçekleşti Lütfen Site Yöneticisiyle iletişime geçin");
                return RedirectToAction(nameof(Index));

            }
            var pathname = await _addImageService.AddImage(model.File);
            model.Photopath = pathname;
            var employeeCreateDto = _mapper.Map<EmployeeCreateDto>(model);
            if (employeeCreateDto.Role == 0)
            {

                employeeCreateDto.Role = DOMAIN.ENUMS.Role.Employee;
            }
            var result = await _employeeService.AddAsync(employeeCreateDto);
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                return RedirectToAction(nameof(Index));
            }
            SuccessNoty(result.Message);
            var user = await _userManager.FindByIdAsync(result.Data.IdentityId.ToString());
            if (user != null)
            {
                await _email.SendPasswordVerificationEmailAsync("E-posta Doğrulama", "E-Posta doğrulamak için", "Account", "VerifyEmail", user, result.Data.Password);
            }
            else
            {
                ErrorNoty("kullanıcı bulunamadı");
                return View();
            }
            if (employeeCreateDto.Isduplicate)
            {
                InformationModal("Aynı İsimden bir başka kullanıcı vardır bu nedenle isminizin sonuna bir sayı eklenmiştir");
                RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> EditEmployee(Guid id)
        {

            var result = await _employeeService.GetByIdAsync(id);
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);

                return View(_mapper.Map<EmployeeUpdateVm>(result.Data));
            }
            var employeeEditVm = _mapper.Map<EmployeeUpdateVm>(result.Data);
            employeeEditVm.DepartmentList = await GetDepartmentSelectListAsync();
            return View(employeeEditVm);

        }
        [HttpPost]
        public async Task<IActionResult> EditEmployee(EmployeeUpdateVm employeeUpdateVms)
        {
            if (!ModelState.IsValid)
            {
                employeeUpdateVms.DepartmentList = await GetDepartmentSelectListAsync();
                return View(employeeUpdateVms);
            }
            var employeeEditDto = _mapper.Map<EmployeeUpdateDto>(employeeUpdateVms);
            employeeEditDto.Photopath = await _addImageService.AddImage(employeeUpdateVms.File);
            employeeEditDto.IsActive = true;
            var result = await _employeeService.UpdateAsync(employeeEditDto);
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                employeeUpdateVms.DepartmentList = await GetDepartmentSelectListAsync();
                return View(employeeUpdateVms);

            }
            SuccessNoty(result.Message);

            var resultVM = _mapper.Map<EmployeeUpdateVm>(result.Data);
            return RedirectToAction(nameof(Index));

        }
        [HttpPost("OrganizationAdmin/Employee/ActiveOrPasive/{Id}/{isActive}")]
        public async Task<IActionResult> ActiveOrPasive(Guid Id, bool isActive)
        {
            var employee = await _employeeService.GetByIdAsync(Id);
            if (!ModelState.IsValid)
            {
                return View(employee);
            }
            var EmployeeEditdto = _mapper.Map<EmployeeUpdateDto>(employee.Data);
            EmployeeEditdto.IsActive = isActive;
            var result = await _employeeService.UpdateAsync(EmployeeEditdto);
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);

                return RedirectToAction(nameof(Index));

            }
            SuccessNoty(result.Message);
            return RedirectToAction(nameof(Index));

        }
        [HttpPost]
        public async Task<ActionResult> Delete(Guid id)
        {
            var result = await _employeeService.DeleteAsync(id);
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                return RedirectToAction(nameof(Index));
            }
            SuccessNoty(result.Message);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> DetailsEmployee(Guid id)
        {
            var result = await _employeeService.GetByIdAsync(id);
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                return View(_mapper.Map<EmployeeDetailsVm>(result.Data));
            }
            SuccessNoty(result.Message);
            var employeeDetailVm = _mapper.Map<EmployeeDetailsVm>(result.Data);
            var department = await _departmentService.GetByIdAsync(result.Data.DepartmentId);
            employeeDetailVm.DepartmentName = department.Data.DepartmentName;
            return View(employeeDetailVm);
        }
        private async Task<SelectList?> GetDepartmentSelectListAsync()
        {
            var department = await _departmentService.GetAllAsync();
            if (!department.IsSuccess)
            {
                return null;
            }
            return new SelectList(department.Data.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.DepartmentName
            }), "Value", "Text");
        }

        public async Task<IActionResult> EmployeeDebit(Guid employeeId)
        {
            //çalışan zimmetleri çekilecek
            var result = await _employeeDebitService.GetAllAsync();
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                return View(_mapper.Map<List<EmployeeDebitListVm>>(result.Data));
            }

            var model = _mapper.Map<List<EmployeeDebitListVm>>(result.Data);
            var employeeDebits = model.Where(x => x.EmployeeId == employeeId).ToList();

            foreach (var item in employeeDebits)
            {
                var orgAsset = await _orgAssetService.GetByIdAsync(item.OrgAssetId);
                item.DebitName = orgAsset.Data.Name;
                var employee = await _employeeService.GetByIdAsync(employeeId);
                item.EmployeeName = employee.Data.Name + " " + employee.Data.Surname;
            }
            SuccessNoty(result.Message);
            return View(employeeDebits);
        }
    }
}
