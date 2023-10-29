using MVC_ONION_PROJECT.APPLICATION.DTo_s.ReimbursementDtos;
using MVC_ONION_PROJECT.DOMAIN.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ONION_PROJECT.APPLICATION.Services.ReimbursementService
{
    public interface IReimbursementService
    {
        Task<IDataResult<ReimbursementDto>> AddAsync(ReimbursementCreateDto reimbursementCreateDto);
        Task<IResult> DeleteAsync(Guid id);
        Task<IDataResult<List<ReimbursementListDto>>> GetAllAsync();
        Task<IDataResult<ReimbursementDto>> GetByIdAsync(Guid? id);
        Task<IDataResult<ReimbursementDto>> UpdateAsync(ReimbursementUpdateDto reimbursementEditDTO);
        Task<IDataResult<ReimbursementDto>> UpdateRecetedAsync(ReimbursementRejectedUpdateDto reimbursementRejectedEditDTO);
        Task<IDataResult<List<ReimbursementListDto>>> GetListOfMineReimbursement(string identityId);

    }
}
