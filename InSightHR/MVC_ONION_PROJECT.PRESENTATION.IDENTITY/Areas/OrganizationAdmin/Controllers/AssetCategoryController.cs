using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.AssetCategoryDtos;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.DepartmentDtos;
using MVC_ONION_PROJECT.APPLICATION.Services.AssetCategoryService;
using MVC_ONION_PROJECT.APPLICATION.Services.DepartmentService;
using MVC_ONION_PROJECT.APPLICATION.Services.OrganizationServices;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.AssetCategoryVms;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.DepartmentVms;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Controllers
{
    public class AssetCategoryController : OrganizationAdminBaseController
    {

        private readonly IAssetCategoryService _assetCategoryService;
        private readonly IMapper _mapper;

        public AssetCategoryController(IAssetCategoryService assetCategoryService, IMapper mapper)
        {
            _assetCategoryService = assetCategoryService;
            _mapper = mapper;
        }


        public async Task<IActionResult> Index()
        {
            var result = await _assetCategoryService.GetAllAsync();
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                return View(_mapper.Map<List<AssetCategoryListVm>>(result.Data));
            }
            var assetCategoryList = _mapper.Map<List<AssetCategoryListVm>>(result.Data);
            return View(assetCategoryList);
        }


        [HttpGet]
        public IActionResult AddAssetCategory()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddAssetCategory(AssetCategoryCreateVm model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var addResult = await _assetCategoryService.AddAsync(_mapper.Map<AssetCategoryCreateDto>(model));
            if (!addResult.IsSuccess)
            {
                ErrorNoty(addResult.Message);
                return View(model);
            }
            SuccessNoty(addResult.Message);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _assetCategoryService.DeleteAsync(id);
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                return RedirectToAction(nameof(Index));
            }
            SuccessNoty(result.Message);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> EditAssetCategory(Guid id)
        {
            var result = await _assetCategoryService.GetByIdAsync(id);
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                return View(_mapper.Map<AssetCategoryUpdateVm>(result.Data));
            }
            var assetCategoryEditVm = _mapper.Map<AssetCategoryUpdateVm>(result.Data);
            return View(assetCategoryEditVm);
        }

        [HttpPost]
        public async Task<IActionResult> EditAssetCategory(AssetCategoryUpdateVm model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var assetCategoryEditDto = _mapper.Map<AssetCategoryUpdateDto>(model);
            var result = await _assetCategoryService.UpdateAsync(assetCategoryEditDto);
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
