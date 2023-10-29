using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Framework;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.DepartmentDtos;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.EmployeeDebitDtos;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.EmployeeDtos;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.OrgAssetDtos;
using MVC_ONION_PROJECT.APPLICATION.Services.AssetCategoryService;
using MVC_ONION_PROJECT.APPLICATION.Services.EmployeeDebitService;
using MVC_ONION_PROJECT.APPLICATION.Services.EmployeeServicces;
using MVC_ONION_PROJECT.APPLICATION.Services.OrgAssetService;
using MVC_ONION_PROJECT.DOMAIN.ENTITIES;
using MVC_ONION_PROJECT.DOMAIN.ENUMS;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.DepartmentVms;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.EmployeeDebitVms;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.EmployeeVms;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.OrgAssetVms;
using System.Collections.Generic;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Controllers
{
    public class EmployeeDebitController : OrganizationAdminBaseController
    {
        private readonly IOrgAssetService _orgAssetService;
        private readonly IAssetCategoryService _assetCategoryService;
        private readonly IEmployeeService _employeeService;
        private readonly IEmployeeDebitService _employeeDebitService;
        private readonly IMapper _mapper;

        public EmployeeDebitController(IOrgAssetService orgAssetService, IMapper mapper, IEmployeeDebitService employeeDebitService, IEmployeeService employeeService, IAssetCategoryService assetCategoryService)
        {
            _orgAssetService = orgAssetService;
            _mapper = mapper;
            _employeeDebitService = employeeDebitService;
            _employeeService = employeeService;
            _assetCategoryService = assetCategoryService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _employeeDebitService.GetAllAsync();
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                return View(_mapper.Map<List<EmployeeDebitListVm>>(result.Data));
            }
            var employeeDebitListVm = _mapper.Map<List<EmployeeDebitListVm>>(result.Data);
            if (employeeDebitListVm.Count <= 0)
            {
                return View();
            }
            else { 

            foreach (var item in employeeDebitListVm)
            {
                var orgAsset = await _orgAssetService.GetByIdAsync(item.OrgAssetId);
                item.DebitName = orgAsset.Data.Name;
                var employee = await _employeeService.GetByIdAsync(item.EmployeeId);
                item.EmployeeName = employee.Data.Name + " " + employee.Data.Surname;
            }

            return View(employeeDebitListVm);
			}
		}

        [HttpGet]
        public async Task<IActionResult> AddEmployeeDebit()
        {
            EmployeeDebitCreateVm vm = new EmployeeDebitCreateVm()
            {
                EmployeeList = await GetEmployeeSelectListAsync(),
                CategoryList = await GetAssetCategorySelectListAsync(),
                AssignmentDate = DateTime.Now
            };
            if (vm.EmployeeList is null|| vm.CategoryList is null)
            {
            if (vm.EmployeeList is null )
                ErrorNoty("Çalışan Olmadan Çalışana Zimmet Atayamazsınız");
            if (vm.CategoryList is null)
                ErrorNoty("Kategory Olmadan Çalışana Zimmet Atayamazsınız");
                return RedirectToAction("index");
            }
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployeeDebit(EmployeeDebitCreateVm model)
        {
            if (!ModelState.IsValid)
            {
                model.EmployeeList = await GetEmployeeSelectListAsync();
                model.CategoryList = await GetAssetCategorySelectListAsync();
                return View(model);
            }

            var addResult = await _employeeDebitService.AddAsync(_mapper.Map<EmployeeDebitCreateDto>(model));
            if (!addResult.IsSuccess)
            {
                ErrorNoty(addResult.Message);
                model.EmployeeList = await GetEmployeeSelectListAsync();
                model.CategoryList = await GetAssetCategorySelectListAsync();
                return View(model);
            }
            SuccessNoty(addResult.Message);
            return RedirectToAction(nameof(Index));
        }

        private async Task<SelectList?  > GetEmployeeSelectListAsync()
        {
            var employee = await _employeeService.GetEmployeesInOrganization();
            if (!employee.IsSuccess)
            {
                return null;
            }
            return new SelectList(employee.Data.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name.ToString() + " " + x.Surname.ToString()
            }), "Value", "Text");
        }

        private async Task<SelectList?> GetAssetCategorySelectListAsync()
        {
            var assetCategory = await _assetCategoryService.GetAllAsync();
            if (!assetCategory.IsSuccess)
            {
                return null;
            }
            return new SelectList(assetCategory.Data.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.CategoryName.ToString()
            }), "Value", "Text");
        }

        [HttpGet]
        public async Task<IActionResult> GetOrgAssetsByCategory(Guid categoryId)
        {
            // categoryId'ye göre ürünleri veritabanından alın veya hesaplayın
            var OrgAssets = await _orgAssetService.GetAllAsync();
            var OrgAssetsByCategory = OrgAssets.Data.Where(x => x.CategoryId == categoryId && x.AssetStatus!=AssetStatus.Assigned && x.AssetStatus!=AssetStatus.PendingApproval).ToList();
            //var products = GetProductsByCategoryFromDatabase(categoryId);

            // Verileri JSON formatında geri döndürün

            var result = OrgAssetsByCategory.Select(p => new { value = p.Id, text = p.Name + " " + "Serial:" + p.SerialNumber });
            return Json(result);
        }

		[HttpPost]
		public async Task<ActionResult> Delete(Guid id)
		{
			var result = await _employeeDebitService.DeleteAsync(id);
			if (!result.IsSuccess)
			{
				ErrorNoty(result.Message);
				return RedirectToAction(nameof(Index));
			}
			SuccessNoty(result.Message);
			return RedirectToAction(nameof(Index));
		}

		[HttpGet]
		public async Task<IActionResult> EditEmployeeDebit(Guid id)
		{
			var result = await _employeeDebitService.GetByIdAsync(id);
			if (!result.IsSuccess)
			{
				ErrorNoty(result.Message);
				return View(_mapper.Map<EmployeeDebitUpdateVm>(result.Data));
			}
			var employeeDebitEditVm = _mapper.Map<EmployeeDebitUpdateVm>(result.Data);
            var employee = await _employeeService.GetByIdAsync(employeeDebitEditVm.EmployeeId);
            var orgAsset = await _orgAssetService.GetByIdAsync(employeeDebitEditVm.OrgAssetId);
            employeeDebitEditVm.DebitName = orgAsset.Data.Name;
            employeeDebitEditVm.EmployeeName = employee.Data.Name+" "+employee.Data.Surname;
            employeeDebitEditVm.ReceiptDate = DateTime.Now;
            return View(employeeDebitEditVm);

		}
		[HttpPost]
		public async Task<IActionResult> EditEmployeeDebit(EmployeeDebitUpdateVm employeeDebitUpdateVm)
		{
            if (!ModelState.IsValid)
            {
                return View(employeeDebitUpdateVm);
            }
            var employeeDebitEditDto = _mapper.Map<EmployeeDebitUpdateDto>(employeeDebitUpdateVm);
            var result = await _employeeDebitService.UpdateAsync(employeeDebitEditDto);
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                return View(employeeDebitUpdateVm);

            }
            SuccessNoty(result.Message);
            return RedirectToAction(nameof(Index));

		}

		public async Task<IActionResult> DetailsEmployeeDebit(Guid id)
		{
			var result = await _employeeDebitService.GetByIdAsync(id);
			if (!result.IsSuccess)
			{
				ErrorNoty(result.Message);
				return View(_mapper.Map<EmployeeDebitDetailsVm>(result.Data));
			}
			SuccessNoty(result.Message);
			var employeeDebitDetailVm = _mapper.Map<EmployeeDebitDetailsVm>(result.Data);
			var employee = await _employeeService.GetByIdAsync(result.Data.EmployeeId);
			employeeDebitDetailVm.EmployeeName = employee.Data.Name+" "+ employee.Data.Surname;
			var orgAsset = await _orgAssetService.GetByIdAsync(result.Data.OrgAssetId);
            employeeDebitDetailVm.DebitName = orgAsset.Data.Name+" "+ orgAsset.Data.SerialNumber;
			return View(employeeDebitDetailVm);
		}

	}
}
