using MVC_ONION_PROJECT.APPLICATION.DTo_s.DepartmentDtos;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.TimeOffDtos;
using MVC_ONION_PROJECT.DOMAIN.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ONION_PROJECT.APPLICATION.Services.TimeOffServices
{
    public interface ITimeOffService
    {
        public Task<IDataResult<TimeOffDto>> AddAsync(TimeOffCreateDto timeOffCreateDto);
        Task<IResult> DeleteAsync(Guid id);
        Task<IDataResult<List<TimeOffListDto>>> GetAllAsync();
        Task<IDataResult<TimeOffDto>> GetByIdAsync(Guid? id);
        Task<IDataResult<TimeOffDto>> UpdateAsync(TimeOffUpdateDto timeOffEditDTO);
        Task<IDataResult<TimeOffDto>> UpdateRecetedAsync(TimeOffRejectedUpdateDto timeOffRejectedEditDTO);
    }
}
