using AutoMapper;
using Microsoft.AspNetCore.Http;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.AssetCategoryDtos;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.DepartmentDtos;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.EmployeeDebitDtos;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.EmployeeDtos;
using MVC_ONION_PROJECT.DOMAIN.ENTITIES;
using MVC_ONION_PROJECT.DOMAIN.ENUMS;
using MVC_ONION_PROJECT.DOMAIN.Utilities;
using MVC_ONION_PROJECT.DOMAIN.Utilities.Results;
using MVC_ONION_PROJECT.INFRASTRUCTURE.Repositories.Concretes;
using MVC_ONION_PROJECT.INFRASTRUCTURE.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ONION_PROJECT.APPLICATION.Services.AssetCategoryService
{
    public class AssetCategoryService : IAssetCategoryService
    {
        private readonly IAssetCategoryRepository _assetCategoryRepository;
        private readonly IOrgAssetRepository _orgAssetRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;


        public AssetCategoryService(IAssetCategoryRepository assetCategoryRepository, IMapper mapper, IOrgAssetRepository orgAssetRepository, IHttpContextAccessor httpContextAccessor)
        {
            _assetCategoryRepository = assetCategoryRepository;
            _mapper = mapper;
            _orgAssetRepository = orgAssetRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IDataResult<AssetCategoryDto>> AddAsync(AssetCategoryCreateDto assetCategoryCreateDto)
        {
            var userOrganizationId = Guid.Parse(_httpContextAccessor.HttpContext?.Session.GetString("OrganizationId"));
            assetCategoryCreateDto.OrganizationId = userOrganizationId;
            if (await _assetCategoryRepository.AnyAsync(x => x.CategoryName == assetCategoryCreateDto.CategoryName && x.OrganizationId== assetCategoryCreateDto.OrganizationId))
            {
                return new ErrorDataResult<AssetCategoryDto>("Bu kategori zaten kayıtlı");
            }
            var assetCategory = _mapper.Map<AssetCategory>(assetCategoryCreateDto);
            await _assetCategoryRepository.AddAsync(assetCategory);
            await _assetCategoryRepository.SaveChangesAsync();
            return new SuccessDataResult<AssetCategoryDto>(_mapper.Map<AssetCategoryDto>(assetCategory), "Kategori başarıyla eklendi");
        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            var assetCategory = await _assetCategoryRepository.GetByIdAsync(id);
            if (assetCategory == null)
            {
                return new ErrorResult("Sistemde kategori bulunamadı.");
            }
            var orgAssets= await _orgAssetRepository.GetAllAsync();
            var orgAssetIsExist = orgAssets.Where(x => x.CategoryId == assetCategory.Id).ToList();
            if(orgAssetIsExist.Count()==0)
            {
                await _assetCategoryRepository.DeleteAsync(assetCategory);
                await _assetCategoryRepository.SaveChangesAsync();
                return new SuccessResult("Silme başarılı");
            }
            else
            {
                return new ErrorResult("Sistemde bu kategoriye bağlı ürünler olduğu için silemezsiniz.");
            }
        }

        public async Task<IDataResult<List<AssetCategoryListDto>>> GetAllAsync()
        {
            var assetCategories = await _assetCategoryRepository.GetAllAsync();
            var userOrganizationId = Guid.Parse(_httpContextAccessor.HttpContext?.Session.GetString("OrganizationId"));
            var orgAssetCategories = assetCategories.Where(x => x.OrganizationId == userOrganizationId).ToList();

            if (orgAssetCategories.Count() <= 0)
            {
                return new ErrorDataResult<List<AssetCategoryListDto>>("Sistemde kayıtlı kategori bulunamadı.");
            }
            var assetCategoryListDto = _mapper.Map<List<AssetCategoryListDto>>(orgAssetCategories);
            return new SuccessDataResult<List<AssetCategoryListDto>>(assetCategoryListDto, "Kategori listeleme başarılı");
        }

        public async Task<IDataResult<AssetCategoryDto>> UpdateAsync(AssetCategoryUpdateDto assetCategoryEditDTO)
        {
            var assetCategory = await _assetCategoryRepository.GetByIdAsync(assetCategoryEditDTO.Id);
            if (assetCategory == null)
            {
                return new ErrorDataResult<AssetCategoryDto>("Kategori bulunamadı.");
            }
            var assetCategories = await _assetCategoryRepository.GetAllAsync();
            var newAssetCategories = assetCategories.ToList();
            newAssetCategories.Remove(assetCategory);
            var updatedAssetCategory = _mapper.Map(assetCategoryEditDTO, assetCategory);
            await _assetCategoryRepository.UpdateAsync(updatedAssetCategory);
            await _assetCategoryRepository.SaveChangesAsync();
            return new SuccessDataResult<AssetCategoryDto>(_mapper.Map<AssetCategoryDto>(updatedAssetCategory), "Kategori güncelleme başarılı");
        }

        public async Task<IDataResult<AssetCategoryDto>> GetByIdAsync(Guid? id)
        {
            var assetCategory = await _assetCategoryRepository.GetByIdAsync(id);
            if (assetCategory == null)
            {
                return new ErrorDataResult<AssetCategoryDto>("Belirtilen ID ile kategori bulunamadı.");
            }
            var assetCategoryDTO = _mapper.Map<AssetCategoryDto>(assetCategory);

            return new SuccessDataResult<AssetCategoryDto>(assetCategoryDTO, "Belirtilen ID'de kategori mevcut.");
        }
    }
}
