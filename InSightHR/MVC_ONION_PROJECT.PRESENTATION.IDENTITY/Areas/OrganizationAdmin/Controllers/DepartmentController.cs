using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.DepartmentDtos;
using MVC_ONION_PROJECT.APPLICATION.Services.DepartmentService;
using MVC_ONION_PROJECT.APPLICATION.Services.EmailHelper;
using MVC_ONION_PROJECT.APPLICATION.Services.EmployeeServicces;
using MVC_ONION_PROJECT.APPLICATION.Services.OrganizationServices;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.DepartmentVms;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Controllers
{
    public class DepartmentController : OrganizationAdminBaseController
    {

        private readonly IDepartmentService _departmentService;
        private readonly IOrganizationService _OrganizationService;
        private readonly IMapper _mapper;
        public DepartmentController(IDepartmentService departmentService, IMapper mapper, IOrganizationService organizationService)
        {
            _departmentService = departmentService;
            _mapper = mapper;
            _OrganizationService = organizationService;
        }
        public async Task<IActionResult> Index()
        {
            var result = await _departmentService.GetAllAsync();
            if (!result.IsSuccess)
            {
               ErrorNoty(result.Message);
                return View(_mapper.Map<List<DepartmentListVm>>(result.Data));
            }
            var departmanlist = _mapper.Map<List<DepartmentListVm>>(result.Data);
            return View(departmanlist);
        }
        [HttpGet]
        public  IActionResult AddDepartment()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddDepartment(DepartmentCreateVm model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var addResult = await _departmentService.AddAsync(_mapper.Map<DepartmentCreateDto>(model));
            if (!addResult.IsSuccess)
            {
               ErrorNoty(addResult.Message);
                return View(model);
            }
           SuccessNoty(addResult.Message);
            return RedirectToAction("index");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _departmentService.DeleteAsync(id);
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                return RedirectToAction(nameof(Index));
            }
            SuccessNoty(result.Message);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> EditDepartment(Guid id)
        {
            var result = await _departmentService.GetByIdAsync(id);
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                return View(_mapper.Map<DepartmentUpdateVms>(result.Data));
            }
            var categoryEditVm = _mapper.Map<DepartmentUpdateVms>(result.Data);
            return View(categoryEditVm);
        }
        [HttpPost]
        public async Task<IActionResult> EditDepartment(DepartmentUpdateVms model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var categoryeditdto = _mapper.Map<DepartmentUpdateDto>(model);
            var result = await _departmentService.UpdateAsync(categoryeditdto);
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                return View(model);
            }
            SuccessNoty(result.Message);
            var resultVM = _mapper.Map<DepartmentUpdateVms>(result.Data);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> DetailsDepartment(Guid id)
        {
            var result = await _departmentService.GetByIdAsync(id);
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                return View(_mapper.Map<DepartmentDetailsVm>(result.Data));
            }
            SuccessNoty(result.Message);
            var departmentDetailVm = _mapper.Map<DepartmentDetailsVm>(result.Data);
            var Organization=await _OrganizationService.GetByIdAsync(result.Data.OrganizationId);
            departmentDetailVm.OrganizationName = Organization.Data.OrganizationName;
            return View(departmentDetailVm);
        }

		public async Task<IActionResult> GetEmployeeList([FromServices] IEmployeeService _employeeService, Guid id)
		{
			var result = await _employeeService.GetAllAsync();
            
			if (!result.IsSuccess)
			{
				ErrorNoty(result.Message);
				return View(_mapper.Map<List<EmployeeListByDepartmentVm>>(result.Data));
			}
			var employeeList = _mapper.Map<List<EmployeeListByDepartmentVm>>(result.Data);
            var employeeByDepartmantList = employeeList.Where(x => x.DepartmentId == id).ToList();
            var department = await _departmentService.GetByIdAsync(id);
            foreach (var item in employeeByDepartmantList)
            {
                item.Department = department.Data.DepartmentName;

			}
			return View(employeeByDepartmantList);
		}
	}
}
