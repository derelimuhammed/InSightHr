using AutoMapper;
using Microsoft.AspNetCore.Http;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.AssetCategoryDtos;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.OrgAssetDtos;
using MVC_ONION_PROJECT.DOMAIN.ENTITIES;
using MVC_ONION_PROJECT.DOMAIN.ENUMS;
using MVC_ONION_PROJECT.DOMAIN.Utilities;
using MVC_ONION_PROJECT.DOMAIN.Utilities.Results;
using MVC_ONION_PROJECT.INFRASTRUCTURE.Repositories.Concretes;
using MVC_ONION_PROJECT.INFRASTRUCTURE.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ONION_PROJECT.APPLICATION.Services.OrgAssetService
{
    public class OrgAssetService : IOrgAssetService
    {
        private readonly IOrgAssetRepository _orgAssetRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _contextAccessor;

        public OrgAssetService(IOrgAssetRepository orgAssetRepository, IMapper mapper, IHttpContextAccessor contextAccessor)
        {
            _orgAssetRepository = orgAssetRepository;
            _mapper = mapper;
            _contextAccessor = contextAccessor;
        }

        public async Task<IDataResult<OrgAssetDto>> AddAsync(OrgAssetCreateDto orgAssetCreateDto)
        {
            if (await _orgAssetRepository.AnyAsync(x => x.SerialNumber == orgAssetCreateDto.SerialNumber))
            {
                return new ErrorDataResult<OrgAssetDto>("Bu demirbaş zaten kayıtlı");
            }
            orgAssetCreateDto.AssetStatus = AssetStatus.NotAssigned;
            var orgAsset = _mapper.Map<OrgAsset>(orgAssetCreateDto);

            await _orgAssetRepository.AddAsync(orgAsset); ;
            await _orgAssetRepository.SaveChangesAsync();
            return new SuccessDataResult<OrgAssetDto>(_mapper.Map<OrgAssetDto>(orgAsset), "Demirbaş başarıyla eklendi");
        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            var orgAsset = await _orgAssetRepository.GetByIdAsync(id);
            if (orgAsset == null)
            {
                return new ErrorResult("Sistemde demirbaş kaydı bulunamadı.");
            }
            else
            {
                await _orgAssetRepository.DeleteAsync(orgAsset);
                await _orgAssetRepository.SaveChangesAsync();
                return new SuccessResult("Silme başarılı");
            }
        }

        public async Task<IDataResult<List<OrgAssetListDto>>> GetAllAsync()
        {
            var assetCategories = await _orgAssetRepository.GetAllAsync();
           var organizationCategories= assetCategories.Where(x => x.Category.OrganizationId == Guid.Parse(_contextAccessor.HttpContext.Session.GetString("OrganizationId")));
            if (organizationCategories.Count() <= 0)
            {
                return new ErrorDataResult<List<OrgAssetListDto>>("Sistemde kayıtlı demirbaş bulunamadı.");
            }
            var orgAssetListDto = _mapper.Map<List<OrgAssetListDto>>(organizationCategories);
            return new SuccessDataResult<List<OrgAssetListDto>>(orgAssetListDto, "Demirbaş listeleme başarılı");
        }

        public async Task<IDataResult<OrgAssetDto>> GetByIdAsync(Guid? id)
        {
            var orgAsset = await _orgAssetRepository.GetByIdAsync(id);
            if (orgAsset == null)
            {
                return new ErrorDataResult<OrgAssetDto>("Belirtilen ID ile demirbaş bulunamadı.");
            }
            var orgAssetDTO = _mapper.Map<OrgAssetDto>(orgAsset);

            return new SuccessDataResult<OrgAssetDto>(orgAssetDTO, "Belirtilen ID'de demirbaş mevcut.");
        }

        public async Task<IDataResult<OrgAssetDto>> UpdateAsync(OrgAssetUpdateDto orgAssetEditDTO)
        {
            var orgAsset = await _orgAssetRepository.GetByIdAsync(orgAssetEditDTO.Id);
            if (orgAsset == null)
            {
                return new ErrorDataResult<OrgAssetDto>("Demirbaş bulunamadı.");
            }
            var orgAssets = await _orgAssetRepository.GetAllAsync();
            var newOrgAssets = orgAssets.ToList();
            newOrgAssets.Remove(orgAsset);
            var updatedOrgAsset = _mapper.Map(orgAssetEditDTO, orgAsset);
            await _orgAssetRepository.UpdateAsync(updatedOrgAsset);
            await _orgAssetRepository.SaveChangesAsync();
            return new SuccessDataResult<OrgAssetDto>(_mapper.Map<OrgAssetDto>(updatedOrgAsset), "Demirbaş güncelleme başarılı");

        }
    }
}
