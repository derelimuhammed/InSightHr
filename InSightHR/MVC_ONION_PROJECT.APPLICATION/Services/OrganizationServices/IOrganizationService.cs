using MVC_ONION_PROJECT.APPLICATION.DTo_s.OrganizationDtos;
using MVC_ONION_PROJECT.DOMAIN.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ONION_PROJECT.APPLICATION.Services.OrganizationServices
{
    public interface IOrganizationService
    {
        Task<IDataResult<OrganizationDto>> AddAsync(OrganizationCreateDto organizationCreateDto);
        Task<IResult> DeleteAsync(Guid id);
        Task<IDataResult<List<OrganizationListDto>>> GetAllAsync();
        Task<IDataResult<OrganizationDto>> GetByIdAsync(Guid id);
        Task<IDataResult<OrganizationDto>> UpdateAsync(OrganizationUpdateDto organizationeditDto);
    }
}
