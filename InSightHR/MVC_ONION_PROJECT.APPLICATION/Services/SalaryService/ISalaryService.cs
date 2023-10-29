using MVC_ONION_PROJECT.APPLICATION.DTo_s.OrgAssetDtos;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.SalaryDtos;
using MVC_ONION_PROJECT.DOMAIN.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ONION_PROJECT.APPLICATION.Services.SalaryService
{
    public interface ISalaryService
    {
        Task<IDataResult<SalaryDto>> AddAsync(SalaryCreateDto salaryCreateDto);
        Task<IResult> DeleteAsync(Guid id);
        Task<IDataResult<List<SalaryListDto>>> GetAllAsync();
        Task<IDataResult<List<SalaryListDto>>> GetAllCurrentAsync();
		Task<IDataResult<List<SalaryListDto>>> GetByEmployeeIdAsync(Guid employeeId);
		Task<IDataResult<SalaryDto>> GetByIdAsync(Guid? id);
        Task<IDataResult<SalaryDto>> UpdateAsync(SalaryUpdateDto salaryEditDto);
    }
}
