using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.AssetCategoryDtos;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.OrgAssetDtos;
using MVC_ONION_PROJECT.APPLICATION.Services.AssetCategoryService;
using MVC_ONION_PROJECT.APPLICATION.Services.EnumHelpers;
using MVC_ONION_PROJECT.APPLICATION.Services.OrgAssetService;
using MVC_ONION_PROJECT.DOMAIN.ENTITIES;
using MVC_ONION_PROJECT.DOMAIN.ENUMS;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.AssetCategoryVms;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.DepartmentVms;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.EmployeeVms;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.OrgAssetVms;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Controllers
{
    public class OrgAssetController : OrganizationAdminBaseController
    {
        private readonly IOrgAssetService _orgAssetService;
        private readonly IAssetCategoryService _assetCategoryService;
        private readonly IEnumHelperService _enumhelper;
        private readonly IMapper _mapper;

        public OrgAssetController(IOrgAssetService orgAssetService, IMapper mapper, IAssetCategoryService assetCategoryService, IEnumHelperService enumhelper)
        {
            _orgAssetService = orgAssetService;
            _mapper = mapper;
            _assetCategoryService = assetCategoryService;
            _enumhelper = enumhelper;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _orgAssetService.GetAllAsync();

            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                return View(_mapper.Map<List<OrgAssetListVm>>(result.Data));
            }
            var orgAssetListVm = _mapper.Map<List<OrgAssetListVm>>(result.Data);
            foreach (var item in orgAssetListVm)
            {
                var category = await _assetCategoryService.GetByIdAsync(item.CategoryId);
                item.CategoryName = category.Data.CategoryName;
            }
           
            return View(orgAssetListVm);

        }

        [HttpGet]
        public async Task<IActionResult> AddOrgAsset()
        {
            OrgAssetCreateVm vm = new OrgAssetCreateVm()
            {
                CategoryList = await GetCategorySelectListAsync(),
                PurchaseDate = DateTime.Now
            };
            if (vm.CategoryList is null)
            {
                ErrorNoty("Kategori Olmadan Organizasyona Demirbaş Atayamazsınız");
                return RedirectToAction("index");
            }
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrgAsset(OrgAssetCreateVm model)
        {
            if (!ModelState.IsValid)
            {
                model.CategoryList = await GetCategorySelectListAsync();
                return View(model);
            }
                
            var addResult = await _orgAssetService.AddAsync(_mapper.Map<OrgAssetCreateDto>(model));
            if (!addResult.IsSuccess)
            {
                ErrorNoty(addResult.Message);
                model.CategoryList = await GetCategorySelectListAsync();
                return View(model);
            }
            SuccessNoty(addResult.Message);
            return RedirectToAction(nameof(Index));
        }

        private async Task<SelectList?> GetCategorySelectListAsync()
        {
            var category = await _assetCategoryService.GetAllAsync();
            if (!category.IsSuccess)
            {
                return null;
            }

            return new SelectList(category.Data.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.CategoryName
            }), "Value", "Text");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _orgAssetService.DeleteAsync(id);
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                return RedirectToAction(nameof(Index));
            }
            SuccessNoty(result.Message);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> EditOrgAsset(Guid id)
        {
            var result = await _orgAssetService.GetByIdAsync(id);
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                return View(_mapper.Map<OrgAssetUpdateVm>(result.Data));
            }
            var orgAssetEditVm = _mapper.Map<OrgAssetUpdateVm>(result.Data);
            orgAssetEditVm.CategoryList= await GetCategorySelectListAsync();
            orgAssetEditVm.AssetStatusList= GetAssetStatusSelectList();
            return View(orgAssetEditVm);
        }

        [HttpPost]
        public async Task<IActionResult> EditOrgAsset(OrgAssetUpdateVm model)
        {
            if (!ModelState.IsValid)
            {
                model.CategoryList = await GetCategorySelectListAsync();
                model.AssetStatusList = GetAssetStatusSelectList();
                return View(model);
            }
                
            var orgAssetEditDto = _mapper.Map<OrgAssetUpdateDto>(model);
            var result = await _orgAssetService.UpdateAsync(orgAssetEditDto);
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                model.CategoryList = await GetCategorySelectListAsync();
                model.AssetStatusList = GetAssetStatusSelectList();
                return View(model);
            }
            SuccessNoty(result.Message);
            return RedirectToAction(nameof(Index));
        }

        private SelectList GetAssetStatusSelectList()
        {
            var enumValues = Enum.GetValues(typeof(AssetStatus))
                            .Cast<AssetStatus>().ToList();

            return new SelectList(enumValues.Select(x => new SelectListItem
            {
                Value = x.ToString(),
                Text = _enumhelper.GetDisplayName(x)
            }), "Value", "Text");
        }

        public async Task<IActionResult> DetailsOrgAsset (Guid id)
        {
            var result = await _orgAssetService.GetByIdAsync(id);
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                return View(_mapper.Map<OrgAssetDetailsVm>(result.Data));
            }
            SuccessNoty(result.Message);
            var orgAssetDetailVm = _mapper.Map<OrgAssetDetailsVm>(result.Data);
            var Category = await _assetCategoryService.GetByIdAsync(result.Data.CategoryId);
            orgAssetDetailVm.CategoryName = Category.Data.CategoryName;
            return View(orgAssetDetailVm);
        }
    }
}
