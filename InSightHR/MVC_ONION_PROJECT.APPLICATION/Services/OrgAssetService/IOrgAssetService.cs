using MVC_ONION_PROJECT.APPLICATION.DTo_s.DepartmentDtos;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.OrgAssetDtos;
using MVC_ONION_PROJECT.DOMAIN.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ONION_PROJECT.APPLICATION.Services.OrgAssetService
{
    public interface IOrgAssetService
    {
        public Task<IDataResult<OrgAssetDto>> AddAsync(OrgAssetCreateDto orgAssetCreateDto);
        Task<IResult> DeleteAsync(Guid id);
        Task<IDataResult<List<OrgAssetListDto>>> GetAllAsync();
        Task<IDataResult<OrgAssetDto>> GetByIdAsync(Guid? id);
        Task<IDataResult<OrgAssetDto>> UpdateAsync(OrgAssetUpdateDto orgAssetEditDTO);
    }
}
