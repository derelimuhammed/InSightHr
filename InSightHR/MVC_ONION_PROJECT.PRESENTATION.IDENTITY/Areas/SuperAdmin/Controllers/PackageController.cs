using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.PackageDtos;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.PackageDtos;
using MVC_ONION_PROJECT.APPLICATION.Services.PackageService;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.SuperAdmin.Models.Package;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.SuperAdmin.Controllers
{
    public class PackageController : SuperAdminBaseController
    {
        private readonly IPackageService _packageService;
        private readonly IMapper _mapper;

        public PackageController(IPackageService packageService, IMapper mapper)
        {
            _packageService = packageService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var result=await _packageService.GetAllAsync();
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                return View(_mapper.Map<List<PackageListVm>>( result.Data));
            }
            return View(_mapper.Map<List<PackageListVm>>(result.Data));
        }
        [HttpGet]
        public IActionResult AddPackage()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddPackage(PackageCreateVm model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var addResult = await _packageService.AddAsync(_mapper.Map<PackageCreateDto>(model));
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
            var result = await _packageService.DeleteAsync(id);
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                return RedirectToAction(nameof(Index));
            }
            SuccessNoty(result.Message);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> EditPackage(Guid id)
        {
            var result = await _packageService.GetByIdAsync(id);
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                return View(_mapper.Map<PackageUpdateVm>(result.Data));
            }
            var packageEditVm = _mapper.Map<PackageUpdateVm>(result.Data);
            return View(packageEditVm);
        }
        [HttpPost]
        public async Task<IActionResult> EditPackage(PackageUpdateVm model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var packageeditdto = _mapper.Map<PackageUpdateDto>(model);
            var result = await _packageService.UpdateAsync(packageeditdto);
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                return View(model);
            }
            SuccessNoty(result.Message);
            var resultVM = _mapper.Map<PackageUpdateVm>(result.Data);
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> ActiveOrPasive(Guid Id, bool isActive)
        {
            var package = await _packageService.GetByIdAsync(Id);
            if (!ModelState.IsValid)
            {
                return View(package);
            }
            var PackageEditdto = _mapper.Map<PackageUpdateDto>(package.Data);
            PackageEditdto.IsActive = isActive;
            var result = await _packageService.UpdateAsync(PackageEditdto);
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);

                return RedirectToAction(nameof(Index));

            }
            SuccessNoty(result.Message);
            return RedirectToAction(nameof(Index));

        }
    }
}
