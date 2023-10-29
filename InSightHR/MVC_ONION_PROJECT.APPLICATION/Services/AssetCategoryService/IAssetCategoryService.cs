using MVC_ONION_PROJECT.APPLICATION.DTo_s.AssetCategoryDtos;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.DepartmentDtos;
using MVC_ONION_PROJECT.DOMAIN.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ONION_PROJECT.APPLICATION.Services.AssetCategoryService
{
    public interface IAssetCategoryService
    {
        public Task<IDataResult<AssetCategoryDto>> AddAsync(AssetCategoryCreateDto assetCategoryCreateDto);
        Task<IResult> DeleteAsync(Guid id);
        Task<IDataResult<List<AssetCategoryListDto>>> GetAllAsync();
        Task<IDataResult<AssetCategoryDto>> UpdateAsync(AssetCategoryUpdateDto assetCategoryEditDTO);
        Task<IDataResult<AssetCategoryDto>> GetByIdAsync(Guid? id);
    }
}
