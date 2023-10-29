using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.OrganizationDtos;
using MVC_ONION_PROJECT.APPLICATION.Services.AddImageServices;
using MVC_ONION_PROJECT.APPLICATION.Services.EmployeeServicces;
using MVC_ONION_PROJECT.APPLICATION.Services.OrganizationServices;
using MVC_ONION_PROJECT.APPLICATION.Services.PackageService;
using MVC_ONION_PROJECT.DOMAIN.ENUMS;
using MVC_ONION_PROJECT.DOMAIN.Utilities.Results;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.SuperAdmin.Models.OrganizationVms;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.SuperAdmin.Models.Package;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.SuperAdmin.Controllers
{
    public class OrganizationController : SuperAdminBaseController
    {
        private readonly IOrganizationService _organizationService;
        private readonly IMapper _mapper;
        private readonly IPackageService _packageService;
        private readonly IAddImageService _addImageService;
        private readonly IEmployeeService _employeeService;

        public OrganizationController(IOrganizationService organizationService, IMapper mapper, IPackageService packageService, IAddImageService addImageService, IEmployeeService employeeService)
        {
            _organizationService = organizationService;
            _mapper = mapper;
            _packageService = packageService;
            _addImageService = addImageService;
            _employeeService = employeeService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _organizationService.GetAllAsync();
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                return View(_mapper.Map<List<OrganizationListVm>>(result.Data));
            }
            var model = _mapper.Map<List<OrganizationListVm>>(result.Data);

            return View(model);
        }
        public async Task<IActionResult> AddOrganization()
        {
            OrganizationCreateVm vm = new OrganizationCreateVm()
            {
                PackageList = await GetPackageSelectListAsync()
            };
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> AddOrganization(OrganizationCreateVm model)
        {
            if (!ModelState.IsValid)
            { 
                model.PackageList= await GetPackageSelectListAsync();
                return View(model); 
            }
            var addResult = await _organizationService.AddAsync(_mapper.Map<OrganizationCreateDto>(model));
            if (!addResult.IsSuccess)
            {
                model.PackageList = await GetPackageSelectListAsync();
                ErrorNoty(addResult.Message);
                return View(model);
            }
            SuccessNoty(addResult.Message);
            return RedirectToAction("index");
        }
        [HttpGet("SuperAdmin/Organization/GetFilter/{filter?}")]
        public async Task<IActionResult> GetFilter(string filter)
        {
            var result = await _organizationService.GetAllAsync();
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                return View(_mapper.Map<List<OrganizationListVm>>(result.Data));
            }
            var model = _mapper.Map<List<OrganizationListVm>>(result.Data);
            if (filter == "Active")
            {
                var filtermodel = model.FindAll(x => x.IsActive == true).ToList();
                return PartialView("_OrganizationListPartial", filtermodel);
            }

            else if (filter == "Inactive")
            {
                var filtermodel = model.FindAll(x => x.IsActive == false).ToList();
                return PartialView("_OrganizationListPartial", filtermodel);
            }
            SuccessNoty(result.Message);
            return PartialView("_OrganizationListPartial", model);
        }
        [HttpPost("SuperAdmin/Organization/ActiveOrPasive/{Id}/{isActive}")]
        public async Task<IActionResult> ActiveOrPasive(Guid Id, bool isActive)
        {
            var organization = await _organizationService.GetByIdAsync(Id);
            if (!ModelState.IsValid)
            {
                return View(organization);
            }
            var OrganizationEditdto = _mapper.Map<OrganizationUpdateDto>(organization.Data);
            OrganizationEditdto.IsActive = isActive;
            var result = await _organizationService.UpdateAsync(OrganizationEditdto);
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
            var result = await _organizationService.DeleteAsync(id);
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                return RedirectToAction(nameof(Index));
            }
            SuccessNoty(result.Message);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> EditOrganization(Guid id)
        {

            var result = await _organizationService.GetByIdAsync(id);
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);

                return View(_mapper.Map<OrganizationUpdateVm>(result.Data));
            }
            var organizationEditVm = _mapper.Map<OrganizationUpdateVm>(result.Data);
            organizationEditVm.PackageList = await GetPackageSelectListAsync();
            return View(organizationEditVm);

        }
        [HttpPost]
        public async Task<IActionResult> EditOrganization(OrganizationUpdateVm organizationUpdateVms)
        {
            if (!ModelState.IsValid)
            {
                organizationUpdateVms.PackageList = await GetPackageSelectListAsync();
                return View(organizationUpdateVms);
            }
            var organizationEditDto = _mapper.Map<OrganizationUpdateDto>(organizationUpdateVms);
            var result = await _organizationService.UpdateAsync(organizationEditDto);
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                organizationUpdateVms.PackageList = await GetPackageSelectListAsync();
                return View(organizationUpdateVms);
            }
            SuccessNoty(result.Message);

            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> DetailsOrganization(Guid id)
        {
            var result = await _organizationService.GetByIdAsync(id);
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                return RedirectToAction("index");
            }
            
            var organizationDetailVm = _mapper.Map<OrganizationDetailsVm>(result.Data);
           var organizationPackageId= result.Data.PackageId.ToString();
            if (!Guid.TryParse(organizationPackageId, out var packageId))
            {
                ErrorNoty("Sistemde Organizasyona ait paket bulunamadı");
                return RedirectToAction("index");
            }   
                var package = await _packageService.GetByIdAsync(packageId);
            if (!package.IsSuccess)
            {
                ErrorNoty(package.Message);
                return RedirectToAction("index");
            }
            
            organizationDetailVm.Package = package.Data;
            var  allemployee=await _employeeService.GetEmployeesInOrganization(id);
            organizationDetailVm.EmployeeOfNumber = allemployee?.Data?.Count()??0;
            if (!allemployee.IsSuccess)
            {
                ErrorNoty(allemployee.Message);
                return View(organizationDetailVm);
            }
          
            SuccessNoty(result.Message);
            return View(organizationDetailVm);
        }



        [HttpGet]
        public async Task<IActionResult> GetPackageByDate(Guid packageId)
        {
            if (packageId.ToString()!= "00000000-0000-0000-0000-000000000000")
            {
                var package = await _packageService.GetByIdAsync(packageId);
                var PackageDurationMonth = TimeSpan.FromDays(package.Data.PackageDurationMonth);
                var result = PackageDurationMonth;
                return Json(result);
            }
            return Json(0);
        }

        //
        private async Task<List<PackageListVm>> GetPackageSelectListAsync()
        {
            var package = await _packageService.GetAllAsync();
            var activepackage=package.Data.Where(x=>x.IsActive==true);

            return _mapper.Map<List<PackageListVm>>(activepackage);
        }
       
    }
}
