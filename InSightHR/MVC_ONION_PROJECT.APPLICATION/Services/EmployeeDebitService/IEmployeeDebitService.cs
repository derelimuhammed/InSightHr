using MVC_ONION_PROJECT.APPLICATION.DTo_s.EmployeeDebitDtos;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.OrgAssetDtos;
using MVC_ONION_PROJECT.DOMAIN.Utilities;

namespace MVC_ONION_PROJECT.APPLICATION.Services.EmployeeDebitService
{
    public interface IEmployeeDebitService
    {
        Task<IDataResult<EmployeeDebitDto>> AddAsync(EmployeeDebitCreateDto employeeDebitCreateDTO);
        Task<IResult> DeleteAsync(Guid id);
        Task<IDataResult<List<EmployeeDebitListDto>>> GetAllAsync();
        Task<IDataResult<EmployeeDebitDto>> GetByIdAsync(Guid id);
        Task<IDataResult<EmployeeDebitDto>> AcceptDebitAsync(Guid id);
        Task<IDataResult<EmployeeDebitDto>> RejectDebitAsync(Guid id);
		Task<IDataResult<List<EmployeeDebitListDto>>> GetByEmployeeId(Guid id);
        Task<IDataResult<EmployeeDebitDto>> UpdateAsync(EmployeeDebitUpdateDto employeeDebitEditDTO);
    }
}