using MVC_ONION_PROJECT.APPLICATION.DTo_s.AdvancePayment;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.AdvancePaymentDtos;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.DepartmentDtos;
using MVC_ONION_PROJECT.DOMAIN.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ONION_PROJECT.APPLICATION.Services.AdvancePaymentService
{
    public interface IAdvancePaymentService
    {
        public Task<IDataResult<AdvancePaymentDto>> AddAsync(AdvancePaymentCreateDto advancePaymentCreateDto);
        Task<IResult> DeleteAsync(Guid id);
        Task<IDataResult<List<AdvancePaymentListDto>>> GetAllAsync();
        Task<IDataResult<AdvancePaymentDto>> GetByIdAsync(Guid? id);
        Task<IDataResult<AdvancePaymentDto>> UpdateAsync(AdvancePaymentUpdateDto advancePaymentEditDTO);
        Task<IDataResult<AdvancePaymentDto>> UpdateRecetedAsync(AdvancePaymentRejectedUpdateDto advancePaymentRejectedEditDTO);
    }
}
