using MVC_ONION_PROJECT.APPLICATION.DTo_s.PackageDtos;
using MVC_ONION_PROJECT.DOMAIN.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ONION_PROJECT.APPLICATION.Services.PackageService
{
    public interface IPackageService
    {
        Task<IDataResult<PackageDto>> AddAsync(PackageCreateDto packageCreateDto);
        Task<IResult> DeleteAsync(Guid id);
        Task<IDataResult<List<PackageListDto>>> GetAllAsync();
        Task<IDataResult<PackageDto>> GetByIdAsync(Guid? id);
        Task<IDataResult<PackageDto>> UpdateAsync(PackageUpdateDto packageeditDto);
    }
}
