using MVC_ONION_PROJECT.APPLICATION.DTo_s;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.DepartmentDtos;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.EmployeeDtos;
using MVC_ONION_PROJECT.DOMAIN.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ONION_PROJECT.APPLICATION.Services.EmployeeServicces
{
    public interface IEmployeeService
    {
        Task<IDataResult<EmployeeDto>> AddAsync(EmployeeCreateDto employeeCreateDto);
        Task<IResult> DeleteAsync(Guid id);
        Task<IDataResult<List<EmployeeListDto>>> GetAllAsync();
        Task<IDataResult<EmployeeDto>> GetByIdAsync(Guid id);
        Task<IDataResult<EmployeeDto>> GetByidentityIdAsync(Guid identityId);
        Task<IDataResult<DepartmentDto>> GetEmployeeOrganization(Guid identityId);
        Task<IDataResult<List<EmployeeListDto>>> GetEmployeesInOrganization();
        Task<IDataResult<List<EmployeeListDto>>> GetEmployeesInOrganization(Guid organizationId);
        Task<IDataResult<EmployeeDto>> UpdateAsync(EmployeeUpdateDto employeeEditDTO);
        
    }
}
