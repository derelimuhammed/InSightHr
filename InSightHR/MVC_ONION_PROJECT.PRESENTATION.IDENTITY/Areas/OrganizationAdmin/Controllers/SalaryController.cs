using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.DepartmentDtos;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.EmployeeDtos;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.OrgAssetDtos;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.SalaryDtos;
using MVC_ONION_PROJECT.APPLICATION.Services.DepartmentService;
using MVC_ONION_PROJECT.APPLICATION.Services.EmployeeServicces;
using MVC_ONION_PROJECT.APPLICATION.Services.SalaryService;
using MVC_ONION_PROJECT.DOMAIN.Utilities.Results;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.DepartmentVms;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.OrgAssetVms;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.SalaryVms;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Vms;
using NuGet.Protocol;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Controllers
{
    public class SalaryController : OrganizationAdminBaseController
    {

        private readonly IEmployeeService _employeeService;
        private readonly ISalaryService _employeeSalaryService;
        private readonly IMapper _mapper;

        public SalaryController(IEmployeeService employeeService, IMapper mapper, ISalaryService employeeSalaryService)
        {
            _employeeService = employeeService;
            _mapper = mapper;
            _employeeSalaryService = employeeSalaryService;
        }


        public async Task<IActionResult> Index()
        {
            var result = await _employeeSalaryService.GetAllCurrentAsync();
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                return View(_mapper.Map<List<SalaryListVm>>(result.Data));
            }
            var salaryListVm = _mapper.Map<List<SalaryListVm>>(result.Data);
            foreach (var item in salaryListVm)
            {
                var employee = await _employeeService.GetByIdAsync(item.EmployeeId);
                item.EmployeeName = employee.Data.Name + " " + employee.Data.Surname;
            }

            return View(salaryListVm);
        }


        public async Task<IActionResult> DetailsEmployeeSalary(Guid id)
        {
            var result = await _employeeSalaryService.GetByEmployeeIdAsync(id);
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                return View(_mapper.Map<List<SalaryListVm>>(result.Data));
            }
            var salaryListVm = _mapper.Map<List<SalaryListVm>>(result.Data);
            foreach (var item in salaryListVm)
            {
                var employee = await _employeeService.GetByIdAsync(item.EmployeeId);
                item.EmployeeName = employee.Data.Name + " " + employee.Data.Surname;
            }
            return View(salaryListVm);

        }


        private async Task<SelectList?> GetEmployeeListAsync()
        {
            var employees = await _employeeService.GetEmployeesInOrganization();
            if (!employees.IsSuccess)
            {
                return null;
            }

            return new SelectList(employees.Data.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name + " " + x.Surname
            }), "Value", "Text");
        }

        public async Task<IActionResult> AddEmployeeSalary()
        {
            SalaryCreateVm vm = new SalaryCreateVm()
            {
                EmployeeList = await GetEmployeeListAsync(),
                SalaryDate = DateTime.Now,
                SalaryEndDate= DateTime.Now
            };
            if (vm.EmployeeList==null)
            {
                ErrorNoty("Çalışan olmadan çalışan bilgisi giremezsiniz");
                return RedirectToAction("index");
            }
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployeeSalary(SalaryCreateVm model)
        {
            if (!ModelState.IsValid)
            {
                model.EmployeeList = await GetEmployeeListAsync();
                return View(model);
            }

            var addResult = await _employeeSalaryService.AddAsync(_mapper.Map<SalaryCreateDto>(model));
            if (!addResult.IsSuccess)
            {
                ErrorNoty(addResult.Message);
                model.EmployeeList = await GetEmployeeListAsync();
                return View(model);
            }
            SuccessNoty(addResult.Message);
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _employeeSalaryService.DeleteAsync(id);
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                return RedirectToAction(nameof(Index));
            }
            SuccessNoty(result.Message);
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> EditEmployeeSalary(Guid id)
        {
            var result = await _employeeSalaryService.GetByIdAsync(id);
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                return View(_mapper.Map<SalaryUpdateVm>(result.Data));
            }
            var employeeSalaryEditVm = _mapper.Map<SalaryUpdateVm>(result.Data);
            var employee = await _employeeService.GetByIdAsync(result.Data.EmployeeId);
            employeeSalaryEditVm.EmployeeName = employee.Data.Name + " " + employee.Data.Surname;
            employeeSalaryEditVm.EmployeeId = result.Data.EmployeeId;
            return View(employeeSalaryEditVm);
        }

        [HttpPost]
        public async Task<IActionResult> EditEmployeeSalary(SalaryUpdateVm model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var salaryEditDto = _mapper.Map<SalaryUpdateDto>(model);
            var result = await _employeeSalaryService.UpdateAsync(salaryEditDto);
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                return View(model);
            }
            SuccessNoty(result.Message);
            return RedirectToAction(nameof(Index));
        }
    }
}
