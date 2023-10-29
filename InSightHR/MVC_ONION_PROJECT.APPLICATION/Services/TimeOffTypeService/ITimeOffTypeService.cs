using MVC_ONION_PROJECT.APPLICATION.DTo_s.TimeOffTypeDtos;
using MVC_ONION_PROJECT.DOMAIN.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ONION_PROJECT.APPLICATION.Services.TimeOffTypeService
{
    public interface ITimeOffTypeService
    {
        public Task<IDataResult<TimeOffTypeDto>> AddAsync(TimeOffTypeCreateDto timeOffTypeCreateDto);
        Task<IResult> DeleteAsync(Guid id);
        Task<IDataResult<List<TimeOffTypeListDto>>> GetAllAsync();
        Task<IDataResult<TimeOffTypeDto>> GetByIdAsync(Guid? id);
        Task<IDataResult<TimeOffTypeDto>> UpdateAsync(TimeOffTypeUpdateDto timeOffTypeEditDTO);
    }
}
